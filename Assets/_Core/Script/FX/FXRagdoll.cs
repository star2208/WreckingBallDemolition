using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Animations.Rigging;
using Sirenix.OdinInspector;

public class FXRagdoll : MonoBehaviour
{
    [FoldoutGroup("Objects")] public Animator animator;
    [FoldoutGroup("Objects")] public RigBuilder rigBuilder;
    [FoldoutGroup("Objects")] public List<Rigidbody> rigidbodies;
    [FoldoutGroup("Objects")] public List<GameObject> detachableObjects;

    [FoldoutGroup("Editor")] public Transform rigRoot;                  

    [FoldoutGroup("Params"),ShowInInspector]
    public bool IsKinematic
    {
        get
        {
            return isKinematic;
        }
        set
        {
            isKinematic = value;
            foreach (var r in rigidbodies)
            {
                r.isKinematic = isKinematic;
            }
        }
    }
    [SerializeField,HideInInspector]
    private bool isKinematic;

    [FoldoutGroup("Params"), ShowInInspector]
    public bool IsTrigger
    {
        get
        {
            return isTrigger;
        }
        set
        {
            isTrigger = value;
            foreach (var r in rigidbodies)
            {
                r.GetComponent<Collider>().isTrigger = isTrigger;
            }
        }
    }
    [SerializeField, HideInInspector]
    private bool isTrigger;

    //所有物理对象层次
    [FoldoutGroup("Params"), ShowInInspector]
    public string LayerName
    {
        get { return layerName; }
        set
        {
            layerName = value;
            foreach (var r in rigidbodies)
            {
                r.gameObject.layer = LayerMask.NameToLayer(layerName);
            }
        }
    }
    private string layerName;

    [FoldoutGroup("Params"), ShowInInspector]
    public CollisionDetectionMode CollisionDetecionMode
    {
        get { return collisionDetecionMode; }
        set
        {
            collisionDetecionMode = value;
            foreach (var r in rigidbodies)
            {
                r.collisionDetectionMode = collisionDetecionMode;
            }
        }
    }
    [SerializeField, HideInInspector]
    private CollisionDetectionMode collisionDetecionMode = CollisionDetectionMode.Discrete;

    [FoldoutGroup("Runtime"),ShowInInspector]
    public bool EnableRagdoll
    {
        get { return enableRagdoll; }
        set
        {
            enableRagdoll = value;

            IsKinematic = !enableRagdoll;
            IsTrigger = !enableRagdoll;
            if (animator) animator.enabled = !enableRagdoll;
            if (rigBuilder) rigBuilder.enabled = !enableRagdoll;

            if (enableRagdoll == true)
            {
                foreach (var g in detachableObjects)
                {
                    if (g.activeSelf == false) return;

                    var ng = Instantiate(g);

                    Destroy(ng.GetComponent<FXUtils>());

                    ng.GetComponent<Rigidbody>().isKinematic = false;
                    ng.transform.position = g.transform.position;
                    ng.transform.rotation = g.transform.rotation;
                    g.SetActive(false);
                }
            }
            else
            {
                foreach(var g in detachableObjects)
                {
                    g.SetActive(true);
                }
            }
        }
    }
    private bool enableRagdoll;

    [FoldoutGroup("Editor"), Button("Find Objects")]
    public void FindObjects ()
    {
        if (rigRoot == null) return;

        rigidbodies.Clear();

        var rs = rigRoot.GetComponentsInChildren<Rigidbody>();
        foreach (var r in rs)
        {
            rigidbodies.Add(r);
        }

        if ( !animator ) animator = GetComponent<Animator>();
    }

    [FoldoutGroup("Editor"), Button("Remove All")]
    public void RemoveAll()
    {
        foreach ( var r in rigidbodies)
        {
            var g = r.gameObject;
            DestroyImmediate(g.GetComponent<Collider>());
            DestroyImmediate(g.GetComponent<Joint>());
            DestroyImmediate(g.GetComponent<Rigidbody>());
        }
    }


}
