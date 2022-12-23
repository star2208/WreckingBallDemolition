using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListUtils
{
    //返回随机元素
    public static T Random<T>(this List<T> list)
    {
        if (list == null || list.Count <= 0) return default;

        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}

