using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Audio Data", menuName = "Starblaze/Audio Data")]
public class AudioData : ScriptableObject
{
    public AudioClip[] clips;

    [FoldoutGroup("Volume and Pitch",expanded:true), Range(0f, 1f)] public float volume = 1f;               //音量
    [FoldoutGroup("Volume and Pitch"), Range(0f,1f)] public float volumeVariant = 0f;                       //音量变化
    [FoldoutGroup("Volume and Pitch"), Range(0f, 2f)] public float pitch = 1f;                              //音调
    [FoldoutGroup("Volume and Pitch"), Range(0f, 1f)] public float pitchVariant = 0f;                       //音调变化

    [FoldoutGroup("Playback")] public AudioMixerGroup mixerGroup;                                           //混合器层
    [FoldoutGroup("Playback")] public bool loop = false;                                                    //是否是循环的
    [FoldoutGroup("Playback")] public bool randomStartPos = false;                                          //是否随机开始播放位置
    [FoldoutGroup("Playback")] public Vector2 delay;                                                        //播放迟延
    [FoldoutGroup("Playback")] public Vector2 fade;                                                         //渐入渐出的时间
    [FoldoutGroup("Playback")] public float spatialBlend = 1.0f;                                            //3D 音效  0: 完全 2D 1: 完全 3D

    //获取列表里一个随机的 clip
    public AudioClip Clip
    {
        get
        {
            return clips[Random.Range(0, clips.Length)];
        }
    }

    //获取随机音量
    public float Volume
    {
        get { return Mathf.Clamp01(volume + Random.Range(-volumeVariant, volumeVariant));}
    }

    //获取随机音调
    public float Pitch
    {
        get { return pitch + Random.Range(-pitchVariant, pitchVariant); }
    }

    //获取随机播放迟延
    public float Delay
    {
        get { return Random.Range(delay.x, delay.y);}
    }

    //混合器层
    public enum MixerGroup
    {
        SoundFX,            //音效
        Music,              //音乐
        UI,                 //UI
        Env,                //环境
        OverlayMusic        //覆盖音乐
    }

    //播放
    public AudioUnit Play()
    {
        return AudioManager.Instance.Play(this);
    }
    public AudioUnit Play( Vector3 position)
    {
        return AudioManager.Instance.Play(this, position);
    }

    public AudioUnit Play(Vector3 position , float volume )
    {
        return AudioManager.Instance.Play(this, position, volume);
    }

}
