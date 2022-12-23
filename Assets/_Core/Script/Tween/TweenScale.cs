using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using DG.Tweening;

public class TweenScale : Tweenable
{

    [FoldoutGroup("Params")] public Vector3 valueIn = Vector2.one;
    [FoldoutGroup("Params")] public Vector3 valueOut = Vector2.zero;
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
    [FoldoutGroup("Params")] public bool x = true;
    [FoldoutGroup("Params")] public bool y = true;
    [FoldoutGroup("Params")] public bool z = true;
    [FoldoutGroup("Params")] public bool relative = false;
    [FoldoutGroup("Params")] public bool activeWhenIn = false;
    [FoldoutGroup("Params")] public bool disactiveWhenOut = false;
    [FoldoutGroup("Params")] public bool destroyWhenOut = false;
    [Space]
    [FoldoutGroup("Params")] public bool ignoreTimeScale = true;
    [FoldoutGroup("Params")] public bool triggerOutWhenInComplete = false;


    [FoldoutGroup("Events")] public UnityEvent onInComplete;
    [FoldoutGroup("Events")] public UnityEvent onOutComplete;

    [Space, FoldoutGroup("Params")]
    public bool state = false;

    private Vector3 OriginScale
    {
        get
        {
            if (originScaleSetted == false)
            {
                originScaleSetted = true;
                originScale = transform.localScale;
            }
            return originScale;
        }
    }
    private Vector3 originScale;
    private bool originScaleSetted = false;

    private Vector3 curValue;
    private Tween tween;

    private RectTransform rectTransform;
    private RectTransform layoutGroup;

    private void Awake()
    {
        //这个用于当改变缩放值时, 强制让 LayoutGroup 更新
        if ( TryGetComponent<RectTransform>(out rectTransform) )
        {
            var horiLayoutGroup = transform.parent.GetComponent<HorizontalLayoutGroup>();
            if (horiLayoutGroup) layoutGroup = horiLayoutGroup.GetComponent<RectTransform>();
        }

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
                if (activeWhenIn == true) gameObject.SetActive(true);

                tween = DOTween.To(() => curValue, x => SetValue(x), valueIn, timeIn)
                    .SetEase(easeIn)
                    .SetDelay(delayIn)
                    .SetUpdate(ignoreTimeScale)
                    .OnComplete(OnInComplete);
             
            }
            else
            {
                tween = DOTween.To(() => curValue, x => SetValue(x), valueOut, timeOut)
                    .SetEase(easeOut)
                    .SetDelay(delayOut)
                    .SetUpdate(ignoreTimeScale)
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

        Vector3 scale = transform.localScale;
        if (x) scale.x = relative ? (value.x + OriginScale.x):(value.x);
        if (y) scale.y = relative ? (value.y + OriginScale.y):(value.y);
        if (z) scale.z = relative ? (value.z + OriginScale.z):(value.z);
        transform.localScale = scale;

        if (layoutGroup) LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup);
    }

    //强制设置状态
    public void ForceSetState(bool state)
    {
        this.state = state;
        SetValue(state ? valueIn : valueOut);
    }

    private void OnInComplete()
    {
        onInComplete.Invoke();

        if (triggerOutWhenInComplete) Out();
    }

    private void OnOutComplete()
    {
        onOutComplete.Invoke();

        if (destroyWhenOut == true) GameObject.Destroy(gameObject);
        if (disactiveWhenOut == true) gameObject.SetActive(false);
    }
}
