using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Player Data", menuName = "Starblaze/Player Data")]
public class PlayerData : ScriptableObject
{
    [FoldoutGroup("Statis")] public int money = 0;
    [FoldoutGroup("Statis")] public int level = 0;

    [FoldoutGroup("Options")] public bool enableSound = true;
    [FoldoutGroup("Options")] public bool enableHaptic = true;
    [FoldoutGroup("Options")] public QualityData.QualityLevelType qualityLevel = QualityData.QualityLevelType.Medium;


    [Button("Save")]
    public void Save()
    {
        ES3.Save<PlayerData>("Player Data", this);
    }

    [Button("Load")]
    public void Load()
    {
        if (ES3.KeyExists("Player Data"))
        {
            ES3.LoadInto<PlayerData>("Player Data", this);
        }

    }

    [Button("Reset")]
    public void Reset()
    {
        money = 0;
        level = 0;

        enableSound = true;
        enableHaptic = true;
        qualityLevel = QualityData.QualityLevelType.Medium;

        Save();
    }
}
