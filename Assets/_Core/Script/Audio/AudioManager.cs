using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using Sirenix.OdinInspector;
public class AudioManager : MonoBehaviour
{

    public const float AUDIO_RANGE_MAX = 40f;                                                   //音效最大传播距离
    public const float AUDIO_RANGE_MAX_SQUARE = 1600.0f;                                        //音效最大传播距离 平方

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (Instantiate(Resources.Load("SYS Audio Manager")) as GameObject).GetComponent<AudioManager>();
            }

            return instance;
        }
    }
    private static AudioManager instance;

    [FoldoutGroup("Category")] public AudioCategory category;

    [FoldoutGroup("Playback")] public bool enableRecycle = true;
    [FoldoutGroup("Playback")] public Transform playingArea;
    [FoldoutGroup("Playback")] public Transform idleArea;

    //混合器
    [FoldoutGroup("Audio Mixer")] public AudioMixer audioMixer;                                 //音乐混合器

    [FoldoutGroup("Audio Mixer"),ShowInInspector]
    public float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = value;
            audioMixer.SetFloat("Music Volume", Mathf.Lerp(-70.0f, 0.0f, musicVolume));
            audioMixer.SetFloat("Overlay Music Volume", Mathf.Lerp(-70.0f, 0.0f, musicVolume));
        }
    }
    private float musicVolume = 1.0f;

    [FoldoutGroup("Audio Mixer"),ShowInInspector]
    public float SfxVolume
    {
        get { return sfxVolume; }
        set
        {
            sfxVolume = value;
            audioMixer.SetFloat("Sound FX Volume", Mathf.Lerp(-70.0f, 0.0f, sfxVolume));
            audioMixer.SetFloat("UI Volume", Mathf.Lerp(-70.0f, 0.0f, sfxVolume));
        }
    }
    private float sfxVolume = 1.0f;

    public float EnvVolume
    {
        get { return envVolume; }
        set
        {
            envVolume = value;
            audioMixer.SetFloat("Env Volume", Mathf.Lerp(-70.0f, 0.0f, envVolume));
        }
    }
    private float envVolume = 1.0f;

    //预制
    [FoldoutGroup("Prefab")] public GameObject audioUnitPrefab;

    [HideInInspector] public List<AudioUnit> audioUnits;                         //所有的 AudioUnit 列表
    [HideInInspector] public Queue<AudioUnit> idleAudioUnits;                    //空闲的 AudioUnit 队列

    //播放队列
    [FoldoutGroup("Play Queue")] public float playQueueGap;                      //播放队列的播放间隔
    [FoldoutGroup("Play Queue")] public int playQueueMaxLength;                  //播放队列最大长度


    //播放队列信息
    public struct PlayQueueInfo
    {
        public Vector2 pos;
        public AudioData audioData;
        public float volume;
    }
    public Queue<PlayQueueInfo> playQueue = new Queue<PlayQueueInfo>();          //播放队列 有些音效需要加入队列依次播放的
    private float playQueueTimer = 0.0f;


    //获取一个空闲的 AudioUnit
    private AudioUnit GetIdleAudioUnit()
    {
        //如果队列里有
        if (enableRecycle && idleAudioUnits != null && idleAudioUnits.Count > 0) return idleAudioUnits.Dequeue();

        //如果没有 创建一个新的
        AudioUnit audioUnit = GameObject.Instantiate(audioUnitPrefab).GetComponent<AudioUnit>();
        audioUnits.Add(audioUnit);

        return audioUnit;
    }

    //获取下一个音效 ID
    private int NextAudioID
    {
        get
        {
            audioID++;
            return audioID;
        }
    }
    private int audioID = 0;

    //播放音效
    public AudioUnit Play(AudioData audioData, Vector3 position, float volume = 1f)
    {
        if (!audioData) return null;

        //获取一个空闲的 AudioUnit
        AudioUnit audioUnit = GetIdleAudioUnit();
        audioUnit.AudioData = audioData;
        audioUnit.Volume = volume;
        audioUnit.Position = position;
        audioUnit.IsPlaying = true;

        return audioUnit;
    }
    public AudioUnit Play(AudioData audioData, float volume = 1.0f)
    {
        return Play(audioData, Vector3.zero, volume);
    }
    public AudioUnit Play (AudioData audioData)
    {
        return Play(audioData, Vector3.zero);
    }
    public AudioUnit Play(string name)
    {
        return Play(category[name]);
    }


    //播放音效 加入队列的
    public void PlayQueued(Vector3 position, AudioData audioData,float volume = 1.0f)
    {
        if (playQueue.Count > playQueueMaxLength) return;
        playQueue.Enqueue(new PlayQueueInfo() {
            pos = position,
            audioData = audioData,
            volume = volume
        });
    }

    private void Awake()
    {
        instance = this;
        idleAudioUnits = new Queue<AudioUnit>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        //播放队列
        if ( playQueue.Count > 0)
        {
            playQueueTimer += Time.deltaTime;
            if (playQueueTimer >= playQueueGap)
            {
                playQueueTimer = 0.0f;
                PlayQueueInfo info = playQueue.Dequeue();

                Play(info.audioData, info.pos, info.volume);
            }
        }
    }
}
