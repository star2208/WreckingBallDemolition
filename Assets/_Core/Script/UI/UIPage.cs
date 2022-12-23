using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class UIPage: MonoBehaviour
{
    [FoldoutGroup("Params")] public bool triggerInOnStart;              //是否在开始时调用 In
    [FoldoutGroup("Params")] public bool activeWhenIn;                  //In 时设为 Active True
    [FoldoutGroup("Params")] public bool disactiveWhenOut;              //Out 时设为 Active Flase
    //[FoldoutGroup("Params")] public bool startState;                  //开始状态是否是 In
    //[FoldoutGroup("Params")] public bool state;                       //当前状态
    //[Space]
    //[FoldoutGroup("Params")] public float blinkTime;

    [FoldoutGroup("Params")] public float delayDisactive = 1.0f;            
    [FoldoutGroup("Params")] public float delayIn;
    [FoldoutGroup("Params")] public float delayOut;

    [FoldoutGroup("Audio")] public AudioData audioIn;
    [FoldoutGroup("Audio")] public AudioData audioOut;

    [FoldoutGroup("Event")] public UnityEvent onIn;
    [FoldoutGroup("Event")] public UnityEvent onOut;

    [FoldoutGroup("Actions"), Button("In")]
    public void In()
    {
        if (activeWhenIn) gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine("In_Internal");
    }

    [FoldoutGroup("Actions"), Button("Out")]
    public void Out()
    {

        StopAllCoroutines();
        StartCoroutine("Out_Internal");
    }

    private IEnumerator In_Internal()
    {
        yield return new WaitForSeconds(delayIn);

        state = true;
        audioIn?.Play();

        onIn.Invoke();
    }

    private IEnumerator Out_Internal()
    {
        yield return new WaitForSeconds(delayOut);

        state = false;
        audioOut?.Play();

        if (disactiveWhenOut) StartCoroutine("Disactive");

        onOut.Invoke();
    }

    private IEnumerator Disactive()
    {
        yield return new WaitForSeconds(delayDisactive);

        gameObject.SetActive(false);
    }

    public bool State
    {
        get { return state; }
        set
        {
            state = value;
            if (state == true) In();
            else Out();
        }
    }
    private bool state;

    //先 In 再 Out
    /*public void Blink()
    {
        if (state == false) In();
        else
        {
            Debug.Log("aaa");
            Out();
            Invoke("In", blinkTime);
        }
    }*/

    private void Start()
    {
        //state = startState;

        if (triggerInOnStart) onIn.Invoke();
    }
}
