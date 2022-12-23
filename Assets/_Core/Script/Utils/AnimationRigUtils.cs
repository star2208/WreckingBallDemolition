using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

using Sirenix.OdinInspector;

#if UNITY_EDITOR
public class AnimationRigUtils : MonoBehaviour
{
    private List<Transform> childrenToAdd;

    [Button("Add All Children")]
    public void AddAllChildren(Transform root)
    {
        var br = GetComponent<BoneRenderer>();
        if (br == null) return;

        childrenToAdd = new List<Transform>();

        AddAllChildren_Internal(root);

        Transform[] newTS = new Transform[childrenToAdd.Count];
        for ( int i = 0;i < childrenToAdd.Count; i++)
        {
            newTS.SetValue(childrenToAdd[i], i);
        }
        br.transforms = newTS;
    }
    public void AddAllChildren_Internal(Transform trans)
    {
        childrenToAdd.Add(trans);
        for (int i = 0; i < trans.childCount; i++)
        {
            AddAllChildren_Internal(trans.GetChild(i));
        }
    }

    [Button("Sync World Pos And Rot")]
    public void SyncWorldPosAndRot( Transform target)
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
#endif