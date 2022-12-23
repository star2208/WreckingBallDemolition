using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using DG.Tweening;

public class TweenParticleEmission : MonoBehaviour
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

    private ParticleSystem ps;

    [Space,FoldoutGroup("Params")]
    public bool state = false;

    private Tween tween;

    private float curValue;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
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

    public bool State
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

    //强制设置状态
    public void ForceSetState( bool state)
    {
        this.state = state;
        SetValue(state ? valueIn : valueOut);
    }

    public void SetValue ( float value)
    {
        curValue = value;

        if (ps)
        {
            var emission = ps.emission;
            emission.rateOverTimeMultiplier = curValue;
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
