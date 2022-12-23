using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;

public class TweenColor : MonoBehaviour
{
    [FoldoutGroup("Params")] public Color valueIn = Color.white;
    [FoldoutGroup("Params")] public Color valueOut = Color.grey;
    [Space]
    [FoldoutGroup("Params")] public float timeIn = 1.0f;
    [FoldoutGroup("Params")] public float timeOut = 1.0f;
    [Space]
    [FoldoutGroup("Params")] public Ease easeIn = Ease.OutCubic;
    [FoldoutGroup("Params")] public Ease easeOut = Ease.OutCubic;
    [Space]
    [FoldoutGroup("Params")] public float delayIn = 0.0f;
    [FoldoutGroup("Params")] public float delayOut = 0.0f;
    [Space]
    [FoldoutGroup("Params")] public bool startState = true;
    [FoldoutGroup("Params")] public bool activeWhenIn = false;
    [FoldoutGroup("Params")] public bool destroyWhenOut = false;
    [FoldoutGroup("Params")] public bool disactiveWhenOut = false;
    [Space]
    [FoldoutGroup("Params")] public bool ignoreTimeScale = true;
    [FoldoutGroup("Params")] public bool ignoreStartState = false;

    [FoldoutGroup("Events")] public UnityEvent onInComplete;
    [FoldoutGroup("Events")] public UnityEvent onOutComplete;

    private TextMeshProUGUI TextUI
    {
        get
        {
            if (textUI == null) textUI = GetComponent<TextMeshProUGUI>();
            return textUI;
        }
    }
    private TextMeshProUGUI textUI;

    private TextMeshPro Text
    {
        get
        {
            if (text == null) text = GetComponent<TextMeshPro>();
            return text;
        }
    }
    private TextMeshPro text;

    private Image Image
    {
        get
        {
            if (image == null) image = GetComponent<Image>();
            return image;
        }
    }
    private Image image;

    [Space,FoldoutGroup("Params")]
    public bool state = false;

    private Tween tween;

    private Color curValue;

    private void Start()
    {
        if (ignoreStartState == false)
        {
            state = startState;

            if (startState == true)
            {
                if (state == true) SetValue(valueIn);
            }
            else
            {
                if (state == false) SetValue(valueOut);
            }
        }
    }

    [ShowInInspector]
    public bool State
    {
        get
        {
            return state;
        }
        set
        {
            //if ( state == value) return;
            state = value;

            //临时处理 用于解决 State 比 Start 先执行的情况
            //startState = state;

            if (activeWhenIn == true) gameObject.SetActive(true);
            else
            {
                if ( gameObject.activeInHierarchy == false )
                {
                    ForceSetState(state);
                    return;
                }
            }

            tween?.Kill();

            if (state == true)
            {

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

    [FoldoutGroup("Actions"), Button("In"), HorizontalGroup("Actions/In Out")]
    public void In()
    {
        if ( state == false )
        {

            SetValue(valueOut);
            State = true;
        }
    }

    [FoldoutGroup("Actions"), Button("Out"), HorizontalGroup("Actions/In Out")]
    public void Out()
    {

        if ( state == true )
        {

            SetValue(valueIn);
            State = false;
        }
    }

    [FoldoutGroup("Actions"), Button("Set Value In"), HorizontalGroup("Actions/Set Value In Out")]
    public void SetValueIn()
    {
        SetValue(valueIn);
    }
    [FoldoutGroup("Actions"), Button("Set Value Out"), HorizontalGroup("Actions/Set Value In Out")]
    public void SetValueOut()
    {
        SetValue(valueOut);
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

    //强制设置状态
    public void ForceSetState( bool state)
    {
        this.state = state;
        SetValue(state ? valueIn : valueOut);
    }

    public void SetValue ( Color value)
    {
        curValue = value;

        if (TextUI) TextUI.color = curValue;
        if (Text) Text.color = curValue;
        if (Image) Image.color = curValue;
    }

    public void Switch()
    {
        if ( state == true)
        {
            Out();
        }
        else
        {
            In();
        }
    }

    private void OnInComplete()
    {
        state = true;
        onInComplete.Invoke();
    }

    private void OnOutComplete()
    {
        state = false;

        if (destroyWhenOut == true) GameObject.Destroy(gameObject);
        if (disactiveWhenOut == true) gameObject.SetActive(false);

        onOutComplete.Invoke();
    }
}
