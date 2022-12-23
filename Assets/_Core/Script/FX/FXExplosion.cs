using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

using Sirenix.OdinInspector;

public class FXExplosion : MonoBehaviour
{
    public LayerMask layerMask = -1;

    public bool explodeOnStart;
    public Vector3 offset;
    public float radius = 5;
    public Vector2 forceRange = new Vector2 ( 500.0f, 1000.0f );
    public float upward = 1;

    [Button("Explode")]
    public void Explode()
    {
        Vector3 wp = transform.TransformPoint(offset);

        Collider[] colliders = Physics.OverlapSphere(wp, radius, layerMask);

        foreach ( var c in colliders)
        {
            c.attachedRigidbody?.AddExplosionForce(forceRange.Random(), wp, radius, upward);
        }

        GetComponent<CinemachineImpulseSource>()?.GenerateImpulse();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 wp = transform.TransformPoint(offset);
        Gizmos.DrawWireSphere(wp, radius);
        Gizmos.DrawWireSphere(wp, 0.02f);
    }

    private void Start()
    {
        if (explodeOnStart) Explode();
    }
}
