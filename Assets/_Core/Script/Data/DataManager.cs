using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (Instantiate(Resources.Load("SYS Data Manager")) as GameObject).GetComponent<DataManager>();
                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }
    private static DataManager instance;

    //预制
    public static PrefabCategory PrefabCategory
    {
        get { return Instance.prefabCategory; }
    }
    public PrefabCategory prefabCategory;

    //全局
    public static GlobalData GlobalData
    {
        get { return Instance.globalData; }
    }
    public GlobalData globalData;

    //玩家存档
    public static PlayerData PlayerData
    {
        get { return Instance.playerData; }
    }
    public PlayerData playerData;
}
