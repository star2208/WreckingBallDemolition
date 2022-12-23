using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using DG.Tweening;

public class TweenOutlineWidth : SerializedMonoBehaviour
{

    [FoldoutGroup("Params")] public float valueIn = 1.0f;
    [FoldoutGroup("Params")] public float valueOut = 0.0f;
    [Space]
    [FoldoutGroup("Params")] public float timeIn = 0.0f;
    [FoldoutGroup("Params")] public float timeOut = 0.0f;
    [Space]
    [FoldoutGroup("Params")] public Ease easeIn = Ease.OutCubic;
    [FoldoutGroup("Params")] public Ease easeOut = Ease.OutCubic;
    [Space]
    [FoldoutGroup("Params")] public float delayIn = 0.0f;
    [FoldoutGroup("Params")] public float delayOut = 0.0f;
    [Space]
    [FoldoutGroup("Params")] public bool startState = false;
    [FoldoutGroup("Params")] public bool enableWhenIn = true;
    [FoldoutGroup("Params")] public bool disableWhenOut = true;
    [FoldoutGroup("Params")] public bool disactiveWhenOut = false;
    [FoldoutGroup("Params")] public bool triggerOutWhenInComplete = false;

    [FoldoutGroup("Events")] public UnityEvent onInComplete;
    [FoldoutGroup("Events")] public UnityEvent onOutComplete;

    [FoldoutGroup("External Params"), ShowInInspector] public ExternalTimeDelegate externalTimeIn;
    [FoldoutGroup("External Params"), ShowInInspector] public ExternalTimeDelegate externalTimeOut;
    public delegate float ExternalTimeDelegate();

    [Space, FoldoutGroup("Params")]
    public bool state = false;

    private float OriginValue
    {
        get
        {
            if (originValueSetted == false)
            {
                originValueSetted = true;

                if (Outline)
                {
                    originValue = Outline.OutlineWidth;
                }
            }
            return originValue;
        }
    }
    private float originValue;
    private bool originValueSetted = false;

    private Outline Outline
    {
        get
        {
            if (outline == null) outline = GetComponent<Outline>();
            return outline;
        }
    }
    private Outline outline;


    private float curValue;
    private Tween tween;
    private bool ignoreStartState = false;

    private void Start()
    {
        if ( ignoreStartState == false)
        {
            if (startState == true)
            {
                state = true;
                SetValue(valueIn);
            }
            else
            {
                state = false;
                SetValue(valueOut);
            }
        }
    }

    public bool State
    {
        get
        {
            return state;
        }
        set
        {
            ignoreStartState = true;

            if ( state == value)
            {
                return;
            }

            if (enableWhenIn == true) Outline.enabled = true;

            state = value;

            tween?.Kill();

            if (state == true)
            {
                float t = timeIn;
                if (externalTimeIn != null) t = externalTimeIn.Invoke();

                tween = DOTween.To(() => curValue, x => SetValue(x), valueIn, t)
                    .SetEase(easeIn)
                    .OnComplete(OnInComplete);
           
            }
            else
            {
                float t = timeOut;
                if (externalTimeOut != null) t = externalTimeOut.Invoke();

                tween = DOTween.To(() => curValue, x => SetValue(x), valueOut, t)
                    .SetEase(easeOut)
                    .OnComplete(OnOutComplete);

                
            }
        }
    }

    [FoldoutGroup("Actions"), Button("In")]
    public void In()
    {
        if ( state == false )
        {
            SetValue(valueOut);
            State = true;
        }
    }

    [FoldoutGroup("Actions"), Button("Out")]
    public void Out()
    {
        if ( state == true )
        {
            SetValue(valueIn);
            State = false;
        }
    }

    public void ForceIn()
    {
        ForceSetState(false);
        In();
    }

    public void ForceOut()
    {
        ForceSetState(true);
        Out();
    }

    private void SetValue ( float value)
    {
        curValue = value;

        if (Outline)
        {
            Outline.OutlineWidth = value;
        }
    }

    //强制设置状态
    public void ForceSetState(bool state)
    {
        this.state = state;
        SetValue( state ? valueIn : valueOut);
    }

    private void OnInComplete()
    {
        state = true;

        if (triggerOutWhenInComplete) Out();

        onInComplete.Invoke();
    }

    private void OnOutComplete()
    {
        state = false;

        onOutComplete.Invoke();

        if (disableWhenOut == true) Outline.enabled = false;
        if (disactiveWhenOut == true) gameObject.SetActive(false);
    }

    public float TimeIn
    {
        get { return timeIn; }
        set { timeIn = value; }
    }

    public float TimeOut
    {
        get { return timeOut; }
        set { timeOut = value; }
    }
}
