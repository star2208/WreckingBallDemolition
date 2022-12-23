using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Tabtale.TTPlugins;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform DebrisArea
    {
        get
        {
            if (debrisArea == null)
            {
                debrisArea = transform.Find("Debris Area");
            }
            return debrisArea;
        }
    }
    private Transform debrisArea;

    public Transform FxArea
    {
        get
        {
            if (fxArea == null)
            {
                fxArea = transform.Find("FX Area");
            }
            return fxArea;
        }
    }
    private Transform fxArea;

    public List<Transform> buildingList = new List<Transform>();
    public Building curBuilding;
    public Transform buildingArea;
    public GameObject viewCam;

    private int buildIndex = 0;
    private Dictionary<string, object> parameters = new Dictionary<string, object>();
    private GameDataManager data;


    private void Awake()
    {
        TTPCore.Setup();
        instance = this;        
        Application.targetFrameRate = DataManager.GlobalData.targetFrameRate;
        curBuilding = buildingList[0].GetComponentInChildren<Building>();
        viewCam.SetActive(false);
    }

    private void Start()
    {
        data = GameDataManager.instance;
        PlayerInput.instance.canOperate = false;

        UIGame.instance.pageGame.gameObject.SetActive(false);
    }

    public void BuildingComplete()
    {
        PlayerInput.instance.canOperate = false;
        UIGamePage.instance.BuildingComplete();
        //GameDataManager.instance.totalMoney += curBuilding.buildingValue;
        Invoke("NextBuilding", 3.0f);

        TTPGameProgression.FirebaseEvents.MissionComplete(parameters);
    }

    public void NextBuilding()
    {
        if (buildIndex >= 3)
        {
            Transform build = CreateBuilding(buildIndex + 1);
            buildingList.Add(build);
        }

        buildingList[0]
            .DOMove(buildingList[0].position + Vector3.left * 1500, 1.5f);
        Destroy(buildingList[0].gameObject, 5f);
        buildingList.RemoveAt(0);
        //buildings[buildIndex].gameObject.SetActive(true);
        buildingList[0]
            .DOLocalMove(Vector3.zero, 1.5f);
        curBuilding = buildingList[0].GetComponentInChildren<Building>();
        //CameraManager.instance.MoveToNextBuilding();
        Invoke("StartNextBuilding", 2.0f);


    }

    private void StartNextBuilding()
    {
        UIGamePage.instance.ShowBuildingVlue();
        PlayerInput.instance.canOperate = true;
        buildIndex++;
        TTPGameProgression.FirebaseEvents.MissionStarted(buildIndex+1, parameters);
    }

    private Transform CreateBuilding(int index)
    {
        Transform trans = Instantiate(data.buildingPre[index % 4], buildingArea.position + Vector3.right * 2000, Quaternion.identity).transform;
        trans.SetParent(buildingArea);
        return trans;
    }

    public void Btn_Start()
    {
        UIGame.instance.pageGame.gameObject.SetActive(true);
        UIGame.instance.pageGame.In();
        PlayerInput.instance.canOperate = true;
        UIGamePage.instance.ShowBuildingVlue();
        viewCam.SetActive(true);

        parameters.Add("missionTest","CPILevel");
        TTPGameProgression.FirebaseEvents.MissionStarted(1, parameters);
    }
}
