using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using DG.Tweening;

public class TweenMPB : SerializedMonoBehaviour
{
    [FoldoutGroup("Params")] public ValueType valueType;
    [FoldoutGroup("Params")] public string valueName;
    [Space]
    [FoldoutGroup("Params")] public Vector4 valueIn = Vector2.zero;
    [FoldoutGroup("Params")] public Vector4 valueOut = Vector2.zero;
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
    [FoldoutGroup("Params")] public bool destroyWhenOut = false;
    [FoldoutGroup("Params")] public bool disactiveWhenOut = false;
    [Space]
    [FoldoutGroup("Params")] public bool ignoreTimeScale = true;
    [FoldoutGroup("Params")] public bool triggerOutWhenInComplete = false;

    [FoldoutGroup("Events")] public UnityEvent onInComplete;
    [FoldoutGroup("Events")] public UnityEvent onOutComplete;

    [FoldoutGroup("External Params"), ShowInInspector] public ExternalTimeDelegate externalTimeIn;
    [FoldoutGroup("External Params"), ShowInInspector] public ExternalTimeDelegate externalTimeOut;
    public delegate float ExternalTimeDelegate();

    [Space, FoldoutGroup("Params")]
    public bool state = false;

    private new Renderer renderer;
    private MaterialPropertyBlock mpb;
    private Vector4 curValue;
    private Tween tween;
    private bool ignoreStartState = false;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();

        mpb = new MaterialPropertyBlock();
        if (renderer) renderer.GetPropertyBlock(mpb);
    }

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

            if (activeWhenIn == true) gameObject.SetActive(true);

            state = value;

            tween?.Kill();

            if (state == true)
            {
                float t = timeIn;
                if (externalTimeIn != null) t = externalTimeIn.Invoke();

                tween = DOTween.To(() => curValue, x => SetValue(x), valueIn, t)
                    .SetEase(easeIn)
                    .SetDelay(delayIn)
                    .SetUpdate(ignoreTimeScale)
                    .OnComplete(OnInComplete);
           
            }
            else
            {
                float t = timeOut;
                if (externalTimeOut != null) t = externalTimeOut.Invoke();

                tween = DOTween.To(() => curValue, x => SetValue(x), valueOut, t)
                    .SetEase(easeOut)
                    .SetDelay(delayOut)
                    .SetUpdate(ignoreTimeScale)
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

    private void SetValue ( Vector4 value)
    {
        curValue = value;

        renderer.GetPropertyBlock(mpb);

        if ( valueType == ValueType.Float)
        {
            mpb.SetFloat(valueName, curValue.x);
        }
        else if ( valueType == ValueType.Color )
        {
            mpb.SetColor(valueName, curValue);
        }
        else if (valueType == ValueType.Vector)
        {
            mpb.SetColor(valueName, curValue);
        }

        renderer.SetPropertyBlock(mpb);
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

        onInComplete?.Invoke();

        if (triggerOutWhenInComplete) Out();
    }

    private void OnOutComplete()
    {
        state = false;

        onOutComplete?.Invoke();

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

    public enum ValueType
    {
        Float,
        Color,
        Vector
    }
}
