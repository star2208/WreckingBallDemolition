using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderTriggerEventReceiver : MonoBehaviour
{
    public LayerMask layerMask;
    [Space]
    public TriggerEvent onTriggerEnter;
    public TriggerEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (layerMask.Includes ( other.gameObject.layer ))
        {
            onTriggerEnter.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (layerMask.Includes(other.gameObject.layer))
        {
            onTriggerExit.Invoke(other);
        }
    }

    [System.Serializable]
    public class TriggerEvent : UnityEvent<Collider>
    {
    }
}
