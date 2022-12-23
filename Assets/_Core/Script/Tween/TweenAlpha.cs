using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;

public class TweenAlpha : Tweenable
{
    [FoldoutGroup("Params")] public float valueIn = 1.0f;
    [FoldoutGroup("Params")] public float valueOut = 0.0f;
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
    [FoldoutGroup("Params")] public bool multiplier = false;
    [Space]
    [FoldoutGroup("Params")] public bool startState = true;
    [FoldoutGroup("Params")] public bool activeWhenIn = false;
    [FoldoutGroup("Params")] public bool disactiveWhenOut = false;
    [FoldoutGroup("Params")] public bool destroyWhenOut = false;
    [Space]
    [FoldoutGroup("Params")] public bool ignoreTimeScale = true;
    [FoldoutGroup("Params")] public bool triggerOutWhenInComplete = false;
    [Space]
    [FoldoutGroup("Events")] public UnityEvent onInComplete;
    [FoldoutGroup("Events")] public UnityEvent onOutComplete;

    private CanvasGroup canvasGroup;
    private Image image;
    private SpriteRenderer spriteRenderer;
    private TextMeshProUGUI text;

    [Space,FoldoutGroup("Params")]
    public bool state = false;

    private Tween tween;

    private float originValue;
    private float curValue;


    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponent<TextMeshProUGUI>();

        if (canvasGroup) originValue = canvasGroup.alpha;
        if (image) originValue = image.color.a;
        if (spriteRenderer) originValue = spriteRenderer.color.a;
        if (text) originValue = text.color.a;
    }

    private void Start()
    {
        state = startState;

        if ( startState == true)
        {
            SetValue(valueIn);
        }
        else
        {
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
            if ( state == value) return;
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

    //强制设置状态
    public void ForceSetState( bool state)
    {
        this.state = state;
        SetValue(state ? valueIn : valueOut);
    }

    public void SetValue ( float value)
    {
        curValue = value;

        if (canvasGroup)
        {
            canvasGroup.alpha = multiplier ? (curValue * originValue) : (curValue);
            return;
        }
        if (image)
        {
            Color c = image.color;
            c.a = multiplier ? (curValue * originValue) : (curValue);
            image.color = c;
            return;
        }
        if (spriteRenderer)
        {
            Color c = spriteRenderer.color;
            c.a = multiplier ? (curValue * originValue) : (curValue);
            spriteRenderer.color = c;
            return;
        }
        if (text)
        {
            Color c = text.color;
            c.a = multiplier ? (curValue * originValue) : (curValue);
            text.color = c;
            return;
        }
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

        if (triggerOutWhenInComplete) Out();
    }

    private void OnOutComplete()
    {
        state = false;

        if (destroyWhenOut == true) GameObject.Destroy(gameObject);
        if (disactiveWhenOut == true) gameObject.SetActive(false);

        onOutComplete.Invoke();
    }
}
