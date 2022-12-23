using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using DG.Tweening;

public class TweenPosition : SerializedMonoBehaviour
{

    [FoldoutGroup("Params")] public Vector3 valueIn = Vector2.zero;
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
    [FoldoutGroup("Params")] public bool activeWhenIn = false;
    [FoldoutGroup("Params")] public bool disactiveWhenOut = false;
    [FoldoutGroup("Params")] public bool destroyWhenOut = false;
    [Space]
    [FoldoutGroup("Params")] public bool x = true;
    [FoldoutGroup("Params")] public bool y = true;
    [FoldoutGroup("Params")] public bool z = true;
    [FoldoutGroup("Params")] public bool relative = false;
    [Space]
    [FoldoutGroup("Params")] public bool ignoreTimeScale = true;
    [FoldoutGroup("Params")] public bool ignoreStartState = false;
    [FoldoutGroup("Params")] public bool triggerOutWhenInComplete = false;

    [FoldoutGroup("Events")] public UnityEvent onInComplete;
    [FoldoutGroup("Events")] public UnityEvent onOutComplete;

    [Space, FoldoutGroup("Runtime")]
    public bool state = false;

    private Vector3 OriginPos
    {
        get
        {
            if (originPosSetted == false)
            {
                originPosSetted = true;

                if (rectTransform)
                {
                    originPos = rectTransform.anchoredPosition;
                }
                else
                {
                    originPos = transform.localPosition;
                }
            }

            return originPos;
        }
    }
    private Vector3 originPos;
    private bool originPosSetted = false;

    private RectTransform RectTransform
    {
        get
        {
            if (rectTransform == null ) rectTransform = GetComponent<RectTransform>();
            return rectTransform;
        }
    }
    private RectTransform rectTransform;

    private Vector3 curValue;
    private Tween tween;

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
            state = value;

            if (activeWhenIn == true) gameObject.SetActive(true);
            else
            {
                if (gameObject.activeInHierarchy == false)
                {
                    ForceSetState(state);
                    return;
                }
            }

            tween?.Kill();

            if (state == true)
            {
                float t = timeIn;
                tween = DOTween.To(() => curValue, x => SetValue(x), valueIn, t)
                    .SetEase(easeIn)
                    .SetUpdate(ignoreTimeScale)
                    .SetDelay(delayIn)
                    .OnComplete(OnInComplete);
           
            }
            else
            {
                float t = timeOut;
                tween = DOTween.To(() => curValue, x => SetValue(x), valueOut, t)
                    .SetEase(easeOut)
                    .SetUpdate(ignoreTimeScale)
                    .SetDelay(delayOut)
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

    private void SetValue ( Vector3 value)
    {
        curValue = value;

        if (RectTransform)
        {
            Vector2 pos = rectTransform.anchoredPosition;
            if (x) pos.x = relative ? (value.x + OriginPos.x) : (value.x);
            if (y) pos.y = relative ? (value.y + OriginPos.y) : (value.y);
            rectTransform.anchoredPosition = pos;
        }
        else
        {
            Vector3 pos = transform.localPosition;
            if (x) pos.x = relative ? (value.x + OriginPos.x) : (value.x);
            if (y) pos.y = relative ? (value.y + OriginPos.y) : (value.y);
            if (z) pos.z = relative ? (value.z + OriginPos.z) : (value.z);
            transform.localPosition = pos;
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

        onInComplete.Invoke();
        if (triggerOutWhenInComplete) Out();
    }

    private void OnOutComplete()
    {
        state = false;

        onOutComplete.Invoke();

        if (destroyWhenOut == true) GameObject.Destroy(gameObject);
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
