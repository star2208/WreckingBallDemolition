using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using DG.Tweening;

public class TweenRotation : Tweenable
{

    [FoldoutGroup("Params")] public Vector3 valueIn = Vector3.zero;
    [FoldoutGroup("Params")] public Vector3 valueOut = Vector3.zero;
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
    [FoldoutGroup("Params")] public bool startState = true;
    [FoldoutGroup("Params")] public bool x = true;
    [FoldoutGroup("Params")] public bool y = true;
    [FoldoutGroup("Params")] public bool z = true;
    [FoldoutGroup("Params")] public bool relative = false;
    [Space]
    [FoldoutGroup("Params")] public bool ignoreTimeScale = true;

    [FoldoutGroup("Events")] public UnityEvent onInComplete;
    [FoldoutGroup("Events")] public UnityEvent onOutComplete;

    [FoldoutGroup("External Params"),ShowInInspector] public ExternalTimeDelegate externalTimeIn;
    [FoldoutGroup("External Params"),ShowInInspector] public ExternalTimeDelegate externalTimeOut;
    public delegate float ExternalTimeDelegate();

    [Space, FoldoutGroup("Params")]
    public bool state = false;

    private Vector3 OriginRot
    {
        get
        {
            if (originRotSetted == false)
            {
                originRotSetted = true;
                originRot = Trans.localRotation.eulerAngles;
            }
            return originRot;
        }
    }
    private Vector3 originRot;
    private bool originRotSetted = false;

    private Transform Trans
    {
        get
        {
            if ( trans == null) trans = GetComponent<Transform>();
            return trans;
        }
    }
    private Transform trans;

    private Vector3 curValue;
    private Tween tween;

    private void Start()
    {

        if ( startState == true)
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

    public override bool State
    {
        get
        {
            return state;
        }
        set
        {
            if ( state == value)
            {
                return;
            }

            state = value;

            tween?.Kill();

            if (state == true)
            {
                float t = timeIn;
                if (externalTimeIn != null) t = externalTimeIn.Invoke();

                tween = DOTween.To(() => curValue, x => SetValue(x), valueIn, t)
                    .SetEase(easeIn)
                    .SetUpdate(ignoreTimeScale)
                    .SetDelay(delayIn)
                    .OnComplete(OnInComplete);
            }
            else
            {
                float t = timeOut;
                if (externalTimeIn != null) t = externalTimeOut.Invoke();

                tween = DOTween.To(() => curValue, x => SetValue(x), valueOut, t)
                    .SetEase(easeOut)
                    .SetUpdate(ignoreTimeScale)
                    .SetDelay(delayOut)
                    .OnComplete(OnOutComplete);

               
            }
        }
    }

    [FoldoutGroup("Actions"), Button("In")]
    public override void In()
    {
        if ( state == false )
        {
            SetValue(valueOut);
            State = true;
        }
    }

    [FoldoutGroup("Actions"), Button("Out")]
    public override void Out()
    {
        if ( state == true )
        {
            SetValue(valueIn);
            State = false;
        }
    }

    public override void ForceIn()
    {
        ForceSetState(false);
        In();
    }

    public override void ForceOut()
    {
        ForceSetState(true);
        Out();
    }

    private void SetValue ( Vector3 value)
    {
        curValue = value;

        Vector3 rot = Trans.localRotation.eulerAngles;
        if (x) rot.x = relative ? (value.x + OriginRot.x):(value.x);
        if (y) rot.y = relative ? (value.y + OriginRot.y):(value.y);
        if (z) rot.z = relative ? (value.z + OriginRot.z):(value.z);
        Trans.localRotation = Quaternion.Euler(rot);
    }

    //强制设置状态
    public void ForceSetState(bool state)
    {
        this.state = state;
        SetValue( state ? valueIn : valueOut);
    }

    private void OnInComplete()
    {
        onInComplete.Invoke();
    }

    private void OnOutComplete()
    {
        onOutComplete.Invoke();
    }
}
