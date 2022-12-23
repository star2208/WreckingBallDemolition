using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColliderUtils
{
    public static Vector3 RandomPoint(this BoxCollider boxCollider )
    {
        Vector3 min = boxCollider.bounds.min;
        Vector3 max = boxCollider.bounds.max;

        Vector3 point;
        point.x = Random.Range(min.x, max.x);
        point.y = Random.Range(min.y, max.y);
        point.z = Random.Range(min.z, max.z);

        return point;
    }
}
