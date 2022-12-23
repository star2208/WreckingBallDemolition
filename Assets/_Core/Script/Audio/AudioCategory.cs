using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

//物品目录
[CreateAssetMenu(fileName = "Audio Data Category", menuName = "Starblaze/Audio Data Category")]
public class AudioCategory : SerializedScriptableObject
{
    //一些全局的音效
    [FoldoutGroup("Audio")] public AudioData hitMarker;
    [FoldoutGroup("Audio")] public AudioData killMarker;


    //所有物品的列表 自动更新
    [AssetList(Path = "_Core/Audio/Audio Data",AutoPopulate =true), OnValueChanged("OnAudioDataListChanged")]
    public List<AudioData> audioDataList;

    public Dictionary<string, AudioData> audioDataDictionary;

    #if UNITY_EDITOR
    [Button("Generate Dictionary")]
    private void OnAudioDataListChanged ()
    {
        if (audioDataDictionary == null)
        {
            audioDataDictionary = new Dictionary<string, AudioData>();
        }
        audioDataDictionary.Clear();

        for ( int i = 0; i < audioDataList.Count;i++)
        {
            if ( audioDataDictionary.ContainsKey (audioDataList[i].name))
            {
                Debug.LogError("Audio Data Category: Diprecated audio name \"" + audioDataList[i].name + "\" at " + i);
            }
            else
            {
                audioDataDictionary.Add(audioDataList[i].name, audioDataList[i]);
            }
        }

        EditorUtility.SetDirty(this);
    }
    #endif

    //获取数据
    public AudioData this[string name]
    {
        get
        {
            if (audioDataDictionary.ContainsKey(name))
            {
                return audioDataDictionary[name];
            }

            return null;
        }
    }

    public AudioData this[int id]
    {
        get
        {
            if (0 <= id && id < audioDataList.Count)
            {
                return audioDataList[id];
            }

            return null;
        }
    }

    #region 加载相关
    //加载音频数据
    public void Load()
    {
        if (isAudioClipsLoaded == true) return;

        int count = audioDataList.Count;
        for ( int i = 0; i < audioDataList.Count; i++)
        {
            foreach (AudioClip ac in audioDataList[i].clips) ac.LoadAudioData();
        }
    }

    private bool isAudioClipsLoaded = false;

    #endregion


}