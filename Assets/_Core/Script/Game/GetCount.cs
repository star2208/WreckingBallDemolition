using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCount : MonoBehaviour
{
    private Building building;

    private void Awake()
    {
        building = transform.parent.GetComponentInChildren<Building>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("HideColl", 0.2f);
    }

    private void HideColl()
    {
        GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fragment"))
        {
            if (!building.fragRemainList.Contains(other.gameObject))
            {
                building.fragRemainList.Add(other.gameObject);
                building.fragJudgeCount++;
            }
        }
    }
}
