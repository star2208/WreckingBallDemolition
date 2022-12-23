using System.Globalization;
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BzKovSoft.RagdollHelper.Editor
{
	/// <summary>
	/// Bone fixer. Main class that draws panel and do a lot of logic
	/// </summary>
	public sealed class BoneHelper : EditorWindow
	{
		[MenuItem("Window/BzSoft/Ragdoll Helper")]
		private static void ShowWindow()
		{
			EditorWindow.GetWindow(typeof(BoneHelper), false, "Ragdoll helper");
		}

		// current selected collider
		int _curPointIndex = -1;

		// variables that initialized in OnEnable()
		GameObject _go;
		Animator _animator;
		Transform _leftKnee;
		Transform _rightKnee;
		Transform _pelvis;
		Dictionary<string, Transform> _symmetricBones;
		
		// variables that initialized in FindColliders() function.
		// you can not do it in OnEnable() function, because,
		// when you rotate colliders, colliders might be
		// reattached to other objects
		Collider[] _colliders;
		Transform[] _transforms;

		RagdollProperties _ragdollProperties = new RagdollProperties
		{
			asTrigger = true,
			rigidAngularDrag = 0.3f,
			rigidDrag = 0.3f
		};
		int _ragdollTotalWeight = 60;           // weight of character (by default 60)

		bool _humanoidSelected;
		PivotMode _lastPivotMode;
		PivotRotation _lastPivotRotation;
		SelectedMode _selectedMode = SelectedMode.Ragdoll;
		SelectedMode _lastSelectedMode;	// to detect, if mode changed from last frame
		readonly string[] _dropDownListOptions = {
			"Ragdoll",
			"Transform Colliders",
			"Transform Joints",
		};
		enum SelectedMode
		{
			Ragdoll,
			Collider,
			ColliderRotate,
			ColliderMove,
			ColliderScale,
			Joints,
		}

		void OnSelectionChange()
		{
			_selectedMode = SelectedMode.Ragdoll;

			if (!GetTarget())
			{
				Repaint();
				return;
			}

			Tools.hidden = false;

			_selectedMode = SelectedMode.Ragdoll;
			if (_humanoidSelected)
			{
				_symmetricBones = FindSymmetricBones(_animator);
				_leftKnee = _animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg);
				_rightKnee = _animator.GetBoneTransform(HumanBodyBones.RightLowerLeg);
				_pelvis = _animator.GetBoneTransform(HumanBodyBones.Hips);
			}
			else
			{
				_symmetricBones = null;
				_leftKnee = null;
				_rightKnee = null;
				_pelvis = null;
			}
			Repaint();
		}

		bool GetTarget()
		{
			_humanoidSelected = false;

			_go = Selection.activeGameObject;
			if (_go == null ||
				EditorUtility.IsPersistent(_go)) // if it is selected as asset, skip it
				return false;

			_animator = _go.GetComponent<Animator>();
			_humanoidSelected = _animator != null && _animator.isHuman;

			return true;
		}

		SelectedMode GetCurrentMode()
		{
			if (_selectedMode == SelectedMode.Collider |
				_selectedMode == SelectedMode.ColliderMove |
				_selectedMode == SelectedMode.ColliderRotate |
				_selectedMode == SelectedMode.ColliderScale)
			{
				switch (Tools.current)
				{
					case Tool.Move:
						_selectedMode = SelectedMode.ColliderMove;
						break;
					case Tool.Rotate:
						_selectedMode = SelectedMode.ColliderRotate;
						break;
					case Tool.Scale:
					case Tool.Rect:
						_selectedMode = SelectedMode.ColliderScale;
						break;
					default:
						_selectedMode = SelectedMode.Collider;
						break;
				}
			}
			return _selectedMode;
		}

		/// <summary>
		/// Find symmetric bones. (e.g. for left arm, it finds right arm and for right leg it finds left leg)
		/// </summary>
		static Dictionary<string, Transform> FindSymmetricBones(Animator animator)
		{
			var symBones = new Dictionary<string, Transform>();

            // feet
            symBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).name, animator.GetBoneTransform(HumanBodyBones.RightLowerLeg));
			symBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg).name, animator.GetBoneTransform(HumanBodyBones.RightUpperLeg));

			symBones.Add(animator.GetBoneTransform(HumanBodyBones.RightLowerLeg).name, animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg));
			symBones.Add(animator.GetBoneTransform(HumanBodyBones.RightUpperLeg).name, animator.GetBoneTransform(HumanBodyBones.LeftUpperLeg));

			// hands
			symBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftLowerArm).name, animator.GetBoneTransform(HumanBodyBones.RightLowerArm));
			symBones.Add(animator.GetBoneTransform(HumanBodyBones.LeftUpperArm).name, animator.GetBoneTransform(HumanBodyBones.RightUpperArm));

			symBones.Add(animator.GetBoneTransform(HumanBodyBones.RightLowerArm).name, animator.GetBoneTransform(HumanBodyBones.LeftLowerArm));
			symBones.Add(animator.GetBoneTransform(HumanBodyBones.RightUpperArm).name, animator.GetBoneTransform(HumanBodyBones.LeftUpperArm));

			return symBones;
		}
		/// <summary>
		/// Find all "CapsuleCollider", "BoxCollider" and "SphereCollider" colliders
		/// </summary>
		void FindColliders()
		{
			if (_go == null)
			{
				_transforms = new Transform[0];
				return;
			}

			var cColliders = _go.GetComponentsInChildren<CapsuleCollider>();
			var bColliders = _go.GetComponentsInChildren<BoxCollider>();
			var sColliders = _go.GetComponentsInChildren<SphereCollider>();
			_colliders = new Collider[cColliders.Length + bColliders.Length + sColliders.Length];
			cColliders.CopyTo(_colliders, 0);
			bColliders.CopyTo(_colliders, cColliders.Length);
			sColliders.CopyTo(_colliders, cColliders.Length + bColliders.Length);

			_transforms = new Transform[_colliders.Length];
			for (int i = 0; i < _colliders.Length; ++i)
			{
				Transform transform = _colliders[i].transform;
				if (transform.name.EndsWith(ColliderHelper.ColliderNodeSufix, false, CultureInfo.InvariantCulture))
					transform = transform.parent;
				_transforms[i] = transform;
			}
		}

		void OnEnable()
		{
			SceneView.onSceneGUIDelegate += OnSceneGUI;
		}
		void OnDisable()
		{
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			Tools.hidden = false;
		}

		void OnSceneGUI(SceneView sceneView)
		{

			CheckSelectedMode();

			if (_humanoidSelected)
				DrawPlayerDirection();

			if (_selectedMode == SelectedMode.ColliderRotate |
				_selectedMode == SelectedMode.ColliderMove |
				_selectedMode == SelectedMode.ColliderScale |
				_selectedMode == SelectedMode.Joints)
			{
				DrawControls();
			}
		}

		void OnGUI()
		{
			CheckSelectedMode();

			DrawPanel();
		}

		/// <summary>
		/// Method intended to be invoked before Drawing GUI
		/// </summary>
		void CheckSelectedMode()
		{
			_selectedMode = GetCurrentMode();

			// if selected item was changed, research colliders
			if (_selectedMode != _lastSelectedMode |
				_lastPivotMode != Tools.pivotMode |
				_lastPivotRotation != Tools.pivotRotation)
			{
				Tools.hidden = _selectedMode != SelectedMode.Ragdoll;

				_lastSelectedMode = _selectedMode;
				_lastPivotMode = Tools.pivotMode;
				_lastPivotRotation = Tools.pivotRotation;
				FindColliders();
				SceneView.RepaintAll();
			}
		}
		/// <summary>
		/// Draw arrow on the screen, forwarded to front of the character
		/// </summary>
		void DrawPlayerDirection()
		{
			float size = HandleUtility.GetHandleSize(_go.transform.position);
			var playerDirection = GetPlayerDirection();
			Color backupColor = Handles.color;
			Handles.color = Color.yellow;
			Handles.ArrowHandleCap(1, _go.transform.position, Quaternion.LookRotation(playerDirection, Vector3.up), size, EventType.Repaint);
			Handles.color = backupColor;
		}
		
		Quaternion _lastRotation;
		bool _buttonPressed;

		/// <summary>
		/// Draws controls of selected mode.
		/// </summary>
		void DrawControls()
		{
			for (int i = 0; i < _transforms.Length; i++)
			{
				Transform transform = _transforms[i];
				Vector3 pos = ColliderHelper.GetRotatorPosition(transform);
				float size = HandleUtility.GetHandleSize(pos);





				if (Handles.Button(pos, Quaternion.identity, size / 6f, size / 6f, Handles.SphereHandleCap))
				{
					_curPointIndex = i;
				
					Quaternion rotatorRotation2 = ColliderHelper.GetRotatorRotarion(transform);
				
					if (!_buttonPressed)
					{
						_lastRotation = rotatorRotation2;
						_buttonPressed = true;
					}
				}
				else
					_buttonPressed = false;

				if (_curPointIndex != i)
					continue;

				// if current point controll was selected
				// draw other controls over it

				Quaternion rotatorRotation = ColliderHelper.GetRotatorRotarion(transform);

				switch (_selectedMode)
				{
					case SelectedMode.ColliderRotate:
						ProcessRotation(rotatorRotation, transform, pos);
						break;
					case SelectedMode.ColliderMove:
						ProcessColliderMove(rotatorRotation, transform, pos);
						break;
					case SelectedMode.ColliderScale:
						ProcessColliderScale(rotatorRotation, transform, pos);
						break;
					case SelectedMode.Joints:
						ProcessJoint(transform);
						break;
				}
			}
		}

		/// <summary>
		/// Rotate node's colider though controls
		/// </summary>
		void ProcessRotation(Quaternion rotatorRotation, Transform transform, Vector3 pos)
		{
			Quaternion newRotation;
			bool changed;

			if (Tools.pivotRotation == PivotRotation.Global)
			{
				Quaternion fromStart = rotatorRotation * Quaternion.Inverse(_lastRotation);
				newRotation = Handles.RotationHandle(fromStart, pos);
				changed = fromStart != newRotation;
				newRotation = newRotation * _lastRotation;
			}
			else
			{
				newRotation = Handles.RotationHandle(rotatorRotation, pos);
				changed = rotatorRotation != newRotation;
			}

			if (changed)
			{
				transform = ColliderHelper.GetRotatorTransform(transform);
				ColliderHelper.RotateCollider(transform, newRotation);
			}
		}

		/// <summary>
		/// Resize collider though controls
		/// </summary>
		/// <param name="transform">The node the collider is attached to</param>
		static void ProcessColliderMove(Quaternion rotatorRotation, Transform transform, Vector3 pos)
		{
			if (Tools.pivotRotation == PivotRotation.Global)
				rotatorRotation = Quaternion.identity;

			Vector3 newPosition = Handles.PositionHandle(pos, rotatorRotation);
			Vector3 translateBy = newPosition - pos;

			if (translateBy != Vector3.zero)
				ColliderHelper.SetColliderPosition(transform, newPosition);
		}

		/// <summary>
		/// Move collider though controls
		/// </summary>
		void ProcessColliderScale(Quaternion rotatorRotation, Transform transform, Vector3 pos)
		{
			float size = HandleUtility.GetHandleSize(pos);
			var collider = ColliderHelper.GetCollider(transform);

			// process each collider type in its own way
			CapsuleCollider cCollider = collider as CapsuleCollider;
			BoxCollider bCollider = collider as BoxCollider;
			SphereCollider sCollider = collider as SphereCollider;

			if (cCollider != null)
			{
				// for capsule collider draw circle and two dot controllers
				Vector3 direction = DirectionIntToVector(cCollider.direction);

				var t = Quaternion.LookRotation(cCollider.transform.TransformDirection(direction));

				// method "Handles.ScaleValueHandle" multiplies size on 0.15f
				// so to send exact size to "Handles.CircleCap",
				// I needed to multiply size on 1f/0.15f
				// Then to get a size a little bigger (to 130%) than
				// collider (for nice looking purpose), I multiply size by 1.3f
				const float magicNumber = 1f / 0.15f * 1.3f;

				// draw radius controll
				
				float radius = Handles.ScaleValueHandle(cCollider.radius, pos, t, cCollider.radius * magicNumber, Handles.CircleHandleCap, 0);
				bool radiusChanged = cCollider.radius != radius;

				Vector3 scaleHeightShift = cCollider.transform.TransformDirection(direction * cCollider.height / 2);

				// draw height controlls
				Vector3 heightControl1Pos = pos + scaleHeightShift;
				Vector3 heightControl2Pos = pos - scaleHeightShift;

				float height1 = Handles.ScaleValueHandle(cCollider.height, heightControl1Pos, t, size * 0.5f, Handles.DotHandleCap, 0);
				float height2 = Handles.ScaleValueHandle(cCollider.height, heightControl2Pos, t, size * 0.5f, Handles.DotHandleCap, 0);
				float newHeight = 0f;
				
				bool moved = false;
				bool firstCtrlSelected = false;
				if (height1 != cCollider.height)
				{
					moved = true;
					firstCtrlSelected = true;
					newHeight = height1;
				}
				else if (height2 != cCollider.height)
				{
					moved = true;
					newHeight = height2;
				}
				
				if (moved | radiusChanged)
				{
					Undo.RecordObject(cCollider, "Resize capsule collider");

					bool upperSelected = false;
					if (moved)
					{
						if (newHeight < 0.01f)
							newHeight = 0.01f;

						bool firstIsUpper = FirstIsUpper(cCollider.transform, heightControl1Pos, heightControl2Pos);
						upperSelected = firstIsUpper == firstCtrlSelected;

						cCollider.center += direction * (newHeight - cCollider.height) / 2 * (firstCtrlSelected ? 1 : -1);
						cCollider.height = newHeight;
					}
					if (radiusChanged)
						cCollider.radius = radius;

					// resize symmetric colliders too
					Transform symBone;
					if (_symmetricBones != null && _symmetricBones.TryGetValue(transform.name, out symBone))
					{
						var symCapsule = ColliderHelper.GetCollider(symBone) as CapsuleCollider;
						if (symCapsule == null)
							return;
						
						Undo.RecordObject(symCapsule, "Resize symetric capsule collider");

						if (moved)
						{
							Vector3 direction2 = DirectionIntToVector(symCapsule.direction);

							Vector3 scaleHeightShift2 = symCapsule.transform.TransformDirection(direction2 * symCapsule.height / 2);
							Vector3 pos2 = ColliderHelper.GetRotatorPosition(symCapsule.transform);

							Vector3 heightControl1Pos2 = pos2 + scaleHeightShift2;
							Vector3 heightControl2Pos2 = pos2 - scaleHeightShift2;

							bool firstIsUpper2 = FirstIsUpper(symCapsule.transform, heightControl1Pos2, heightControl2Pos2);

							symCapsule.center += direction2 * (newHeight - symCapsule.height) / 2
								* (upperSelected ? 1 : -1)
								* (firstIsUpper2 ? 1 : -1);

							symCapsule.height = cCollider.height;
						}
						if (radiusChanged)
							symCapsule.radius = cCollider.radius;
					}
				}
			}
			else if (bCollider != null)
			{
				// resize Box collider

				var newSize = Handles.ScaleHandle(bCollider.size, pos, rotatorRotation, size);
				if (bCollider.size != newSize)
				{
					Undo.RecordObject(bCollider, "Resize box collider");
					bCollider.size = newSize;
				}
			}
			else if (sCollider != null)
			{
				// resize Sphere collider
				var newRadius = Handles.RadiusHandle(rotatorRotation, pos, sCollider.radius, true);
				if (sCollider.radius != newRadius)
				{
					Undo.RecordObject(sCollider, "Resize sphere collider");
					sCollider.radius = newRadius;
				}
			}
			else
				throw new InvalidOperationException("Unsupported Collider type: " + collider.GetType().FullName);
		}

		private static bool FirstIsUpper(Transform transform, Vector3 heightControl1Pos, Vector3 heightControl2Pos)
		{
			if (transform.parent == null)
				return true;

			Vector3 currentPos = transform.position;
			Vector3 parentPos;
			do
			{
				transform = transform.parent;
				parentPos = transform.position;
			}
			while (parentPos == currentPos & transform.parent != null);

			if (parentPos == currentPos)
				return true;

			Vector3 limbDirection = currentPos - parentPos;

			limbDirection.Normalize();

			float d1 = Vector3.Dot(limbDirection, heightControl1Pos - parentPos);
			float d2 = Vector3.Dot(limbDirection, heightControl2Pos - parentPos);

			
			bool firstIsUpper = d1 < d2;
			return firstIsUpper;
		}

		/// <summary>
		/// Int (Physx spesific) direction to Vector3 direction
		/// </summary>
		static Vector3 DirectionIntToVector(int direction)
		{
			Vector3 v;
			switch (direction)
			{
				case 0:
					v = Vector3.right;
					break;
				case 1:
					v = Vector3.up;
					break;
				case 2:
					v = Vector3.forward;
					break;
				default:
					throw new InvalidOperationException();
			}
			return v;
		}

		static void ProcessJoint(Transform transform)
		{
			CharacterJoint joint = transform.GetComponent<CharacterJoint>();
			if (joint == null)
				return;

			JointController.DrawControllers(joint);
		}
		/// <summary>
		/// Determines and return character's face direction
		/// </summary>
		Vector3 GetPlayerDirection()
		{
			Vector3 leftKnee = _leftKnee.transform.position - _pelvis.transform.position;
			Vector3 rightKnee = _rightKnee.transform.position - _pelvis.transform.position;

			return Vector3.Cross(leftKnee, rightKnee).normalized;
		}
		/// <summary>
		/// Draw application form panel
		/// </summary>
		void DrawPanel()
		{
			// set the form more transparent if "NoAction" is selected
			// set size of form for each selected mode

			// draw panel
			Handles.BeginGUI();

			// draw list of modes to select
			GUIStyle style;
			style = new GUIStyle(GUI.skin.label);
			style.normal.textColor = Color.blue;
			style.alignment = TextAnchor.UpperCenter;

			GUILayout.Label(_humanoidSelected ? "Humanoid selected" : "Simple object selected", style);
			SelectedOptionToEnum(GUILayout.SelectionGrid(SelectedOptionFromEnum(), _dropDownListOptions, 1, EditorStyles.radioButton));

			// for Ragdoll mode draw additional controlls
			if (_selectedMode == SelectedMode.Ragdoll)
				DrawRagdollPanel();

			Handles.EndGUI();
		}

		int SelectedOptionFromEnum()
		{
			switch (_selectedMode)
			{
				case SelectedMode.Ragdoll:
					return 0;
				case SelectedMode.Collider:
				case SelectedMode.ColliderMove:
				case SelectedMode.ColliderRotate:
				case SelectedMode.ColliderScale:
					return 1;
				case SelectedMode.Joints:
					return 2;
				default: throw new InvalidOperationException();
			}
		}

		void SelectedOptionToEnum(int option)
		{
			switch (option)
			{
				case 0:
					_selectedMode = SelectedMode.Ragdoll;
					break;
				case 1:
					_selectedMode = SelectedMode.Collider;
					break;
				case 2:
					_selectedMode = SelectedMode.Joints;
					break;
				default: throw new InvalidOperationException();
			}
		}

		void DrawRagdollPanel()
		{
			GUILayout.BeginVertical("box");
			
			if (_humanoidSelected)
			{
				GUILayout.Label("Ragdoll:");
				if (GUILayout.Button("Create"))
					CreateRagdoll();
				if (GUILayout.Button("Remove"))
					RemoveRagdoll();

				_ragdollTotalWeight = EditorGUILayout.IntField("Total Weight:", _ragdollTotalWeight);

				_ragdollProperties.Draw();
			}
			else
			{
				GUILayout.Label("Ragdoll creator supported only for humanoids");
			}

			GUILayout.EndVertical();
		}
		/// <summary>
		/// Remove all colliders, joints, and rigids from "_go" object
		/// </summary>
		void RemoveRagdoll()
		{
			Ragdoller ragdoller = new Ragdoller(_go.transform, GetPlayerDirection());
			ragdoller.ClearRagdoll();
		}
		/// <summary>
		/// Create Ragdoll components on _go object
		/// </summary>
		void CreateRagdoll()
		{
			Ragdoller ragdoller = new Ragdoller(_go.transform, GetPlayerDirection());
			ragdoller.ApplyRagdoll(_ragdollTotalWeight, _ragdollProperties);
		}
	}
}