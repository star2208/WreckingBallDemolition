using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;


[CreateAssetMenu(fileName = "Quality Data", menuName = "Starblaze/Quality Data")]
public class QualityData : ScriptableObject
{


    public List<QualityInfo> qualityInfos;

    [FoldoutGroup("Runtime"), ShowInInspector]
    public QualityLevelType QualityLevel
    {
        get { return qualityLevel; }
        set
        {
            qualityLevel = value;

            QualitySettings.SetQualityLevel((int)qualityLevel, false);

            if (CameraManager.instance) CameraManager.instance.postProcessVolume.profile = qualityInfos[(int)qualityLevel].profile;
        }
    }
    private QualityLevelType qualityLevel;

    //设置当前的画面等级
    public void SetQuality( string quality)
    {
        var qi = qualityInfos.Find(x => (x.level.ToString() == quality));
        if ( qi != null )
        {
            QualityLevel = qi.level;
        }
    }

    public void SetQuality()
    {
        QualityLevel = qualityLevel;
    }

    public enum QualityLevelType
    {
        Low,
        Medium,
        High,
        Ultra
    }


    [System.Serializable]
    public class QualityInfo
    {
        public QualityLevelType level;
        public VolumeProfile profile;
    }

}
