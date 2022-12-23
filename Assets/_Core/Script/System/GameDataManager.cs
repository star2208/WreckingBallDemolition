using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager instance;

    [FoldoutGroup("Money")] public int totalMoney = 13500;
    [FoldoutGroup("Money")] public int sizeCost = 190;
    [FoldoutGroup("Money")] public int powerCost = 190;
    [FoldoutGroup("Money")] public int incomeCost = 190;

    [FoldoutGroup("Param")] public int hitRange = 10;
    [FoldoutGroup("Param")] public float bombStrength = 0.5f;
    [FoldoutGroup("Param")] public int hitIncome = 10;
    [FoldoutGroup("Param")] public int sizeLevel = 1;
    [FoldoutGroup("Param")] public int powerLevel = 1;
    [FoldoutGroup("Param")] public int incomeLevel = 1;
    [FoldoutGroup("Param")] public int sizeAdd = 1;
    [FoldoutGroup("Param")] public int powerAdd = 1;
    [FoldoutGroup("Param")] public int incomeAdd = 1;

    [FoldoutGroup("Pre")] public GameObject hitEffectPre;
    [FoldoutGroup("Pre")] public GameObject uiWarningPre;
    [FoldoutGroup("Pre")] public GameObject hammerPre;
    [FoldoutGroup("Pre")] public GameObject hitBoxPre;
    [FoldoutGroup("Pre")] public GameObject bombPre;
    [FoldoutGroup("Pre")] public GameObject dollarPre;
    [FoldoutGroup("Pre")] public GameObject dollarLPre;
    [FoldoutGroup("Pre")] public GameObject[] buildingPre;

    [FoldoutGroup("Audio")] public AudioSource audio_Button;
    [FoldoutGroup("Audio")] public AudioSource audio_Money;
    [FoldoutGroup("Audio")] public AudioSource audio_MoneyL;
    [FoldoutGroup("Audio")] public AudioSource audio_Break;
    [FoldoutGroup("Audio")] public GameObject audio_HitPre;
    [FoldoutGroup("Audio")] public AudioClip[] hitClips;
    [FoldoutGroup("Audio")] public List<AudioSource> hitList = new List<AudioSource>();
    [FoldoutGroup("Audio")] public int hitIndex = 0;
    [FoldoutGroup("Audio")] public AudioClip[] breakClips;
    [FoldoutGroup("Audio")] public List<AudioSource> breakList = new List<AudioSource>();
    [FoldoutGroup("Audio")] public int breakIndex = 0;
    [FoldoutGroup("Audio")] public bool takeBreakAudio = false;

    private void Awake()
    {
        instance = this;
    }

    private float timer = 0;
    public bool canTakeBreakAudio = true;

    private void Update()
    {

    }
}
