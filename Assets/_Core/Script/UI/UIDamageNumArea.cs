using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//显示伤害数字的
public class UIDamageNumArea : MonoBehaviour
{
    [FoldoutGroup("Prefab")] public GameObject damageNumPrefab;

    //添加一个
    //参数 伤害 世界位置 是否是暴击
    [FoldoutGroup("Runtime"), Button("Add")]
    public void Add ( int damage, Vector3 worldPos, bool critical)
    {
        var dn = Instantiate(damageNumPrefab, transform);
        dn.GetComponent<UIDamageNum>().Set(damage, worldPos, critical);
    }

    //添加一个
    //参数 伤害 伤害数字抖动百分比 世界位置 是否是暴击
    [FoldoutGroup("Runtime"), Button("Add")]
    public void Add(int damage, float jitter, Vector3 worldPos, bool critical)
    {
        int j = Mathf.RoundToInt((float)damage * jitter * 2.0f * (Random.value - 0.5f));

        var dn = Instantiate(damageNumPrefab, transform);
        dn.GetComponent<UIDamageNum>().Set(damage + j, worldPos, critical);
    }
}
