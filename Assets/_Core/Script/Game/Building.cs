using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool allowBothSide = false;

    public Transform topTrans;
    public Transform hammerConnectTrans;

    public int buildingValue;
    public float destroyPer;

    public int fragHP;//碎片血量

    public List<GameObject> fragRemainList = new List<GameObject>();
    public int fragJudgeCount;
    public Transform fragArea;

    private MeshRenderer[] fragGos;

    private void Awake()
    {
        fragGos = transform.GetComponentsInChildren<MeshRenderer>();
        foreach (var go in fragGos)
        {
            go.gameObject.AddComponent<BuildingFrag>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private bool isComplete = false;

    // Update is called once per frame
    void Update()
    {
        if (isComplete)
            return;

        destroyPer = (1 - ((float)fragRemainList.Count / (float)fragJudgeCount));
        if (destroyPer >= 0.92f)
        {
            //执行结束逻辑
            isComplete = true;
            GameManager.instance.BuildingComplete();
        }
    }

    public void GetPosition()
    {
        if (fragRemainList.Count <= 0)
            return;

        int rand = Random.Range(0, fragRemainList.Count);
        PlayerInput.instance.CreateHammer(Camera.main.WorldToScreenPoint(fragRemainList[rand].transform.position));
    }
}
