using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

//音效单位  
public class AudioUnit : MonoBehaviour
{
    public int id = -1;

    //当前的音效数据
    public AudioData AudioData
    {
        get { return audioData; }
        set
        {
            audioData = value;

            dataVolume = audioData.Volume;
            dataPitch = audioData.Pitch;

            AudioSource.clip = audioData.Clip;
            AudioSource.pitch = pitch * dataPitch;
            AudioSource.loop = audioData.loop;
            AudioSource.spatialBlend = AudioData.spatialBlend;
            AudioSource.outputAudioMixerGroup = audioData.mixerGroup;

            //播放时间位置
            if (AudioData.randomStartPos) AudioSource.timeSamples = Mathf.FloorToInt(UnityEngine.Random.value * AudioSource.clip.samples);
            else AudioSource.timeSamples = 0;

            if (audioData.fade.x > 0.0f) fadeVolumeMultiplier = 0.0f;
        }
    }
    private AudioData audioData;

    //音效源
    public AudioSource AudioSource
    {
        get
        {
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            return audioSource;
        }
    }
    private AudioSource audioSource;

    //跟随的对象
    public Transform Target
    {
        get { return target; }
        set
        {
            target = value;
        }

    }
    private Transform target;

    //音量
    [ShowInInspector]
    public float Volume
    {
        get { return volume; }
        set
        {
            volume = value;
            audioSource.volume = fadeVolumeMultiplier * dataVolume * volume;
        }
    }
    private float volume = 1.0f;                        //当前实例的音量
    private float dataVolume = 1.0f;                    //音效数据的音量
    private float fadeVolumeMultiplier = 1.0f;          //渐入渐出的系数

    public float Pitch
    {
        get { return pitch; }
        set
        {
            pitch = value;
            audioSource.pitch = pitch * dataPitch;
        }
    }
    private float pitch = 1.0f;                         //当前实例的音调
    private float dataPitch = 1.0f;                     //音效数据的音调

    //位置
    public Vector3 Position
    {
        get { return transform.position; }
        set
        {
            transform.position = value;
        }
    }

    //是否在播放
    public bool IsPlaying
    {
        get { return AudioSource.isPlaying; }
        set
        {
            //播放
            if (value == true)
            {
                if (AudioData == null) return;

                fadeVolumeMultiplier = 1.0f;
                playingTimer = 0.0f;
                Volume = volume;

                curDelay = AudioData.Delay;
                AudioSource.enabled = true;
                AudioSource.PlayDelayed(curDelay);

                if (AudioSource.loop == false) Invoke("OnAudioFinishPlaying", curDelay + AudioSource.clip.length * AudioSource.pitch);

                transform.SetParent(AudioManager.Instance.playingArea);
            }
            //停止
            else
            {
                //如果是渐出的
                if( audioData.fade.y > 0.0f)
                {
                    isFadingOut = true;
                    fadingOutTimer = 0.0f;
                }
                //否则 直接停止
                else
                {
                    OnAudioFinishPlaying();
                    AudioSource.Stop();
                }
            }
        }
    }
    public void Play() { IsPlaying = true; }
    public void Stop() { IsPlaying = false; }

    [ShowInInspector, DisplayAsString] private float playingTimer = 0.0f;                  //播放时的计时器
    [ShowInInspector, DisplayAsString] private float fadingOutTimer = 0.0f;                //渐出状态计时器
    [ShowInInspector, DisplayAsString] private bool isFadingOut = false;                   //是否在渐出状态
    [ShowInInspector, DisplayAsString] private float curDelay = 0.0f;                      //当前的播放迟延

    //当音效播放完毕
    private void OnAudioFinishPlaying()
    {
        CancelInvoke();

        id = -1;
        target = null;
        volume = 1.0f;
        pitch = 1.0f;
        fadeVolumeMultiplier = 1.0f;


        //回收
        if (AudioManager.Instance.enableRecycle)
        {
            AudioSource.enabled = false;
            AudioManager.Instance.idleAudioUnits.Enqueue(this);
            transform.SetParent(AudioManager.Instance.idleArea);
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }

    //更新
    private void Update()
    {
        //播放中
        if( IsPlaying )
        {
            playingTimer += Time.deltaTime;

            //跟随目标
            if (Target) Position = Target.position;

            //淡入
            if (audioData.fade.x > 0.0f && playingTimer < audioData.fade.x )
            {
                fadeVolumeMultiplier = playingTimer / audioData.fade.x;
                Volume = volume;
            }
        }


        //非循环的 带淡出的 自动在快结束前触发淡出
        if( audioData.fade.y > 0 && audioData.loop == false )
        {
            if (AudioSource.clip.length - AudioSource.time <= audioData.fade.y) isFadingOut = true;
        }
        
        //淡出中
        if( IsPlaying && isFadingOut )
        {
            fadingOutTimer += Time.deltaTime;

            fadeVolumeMultiplier = (audioData.fade.y - fadingOutTimer) / audioData.fade.y;
            Volume = volume;

            if ( fadingOutTimer >= audioData.fade.y )
            {
                isFadingOut = false;
                fadingOutTimer = 0.0f;
                OnAudioFinishPlaying();
                AudioSource.Stop();
            }
        }
    }

}
