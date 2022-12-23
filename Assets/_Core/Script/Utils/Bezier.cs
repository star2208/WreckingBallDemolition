using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour
{
    public static Vector3 Quad ( Vector3 p0, Vector3 p1 , Vector3 p2, float t)
    {
        return (1 - t) * (1 - t) * p0 +
            2 * (1 - t) * t * p1 +
            t * t * p2;
    }
}
