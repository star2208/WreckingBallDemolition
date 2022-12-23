using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using DG.Tweening;
using TMPro;

public class TweenActive : Tweenable
{
    [FoldoutGroup("Params")] public float timeIn = 1.0f;
    [FoldoutGroup("Params")] public float timeOut = 1.0f;
    [Space]
    [FoldoutGroup("Params")] public bool ignoreTimeScale = true;
    [Space]
    [FoldoutGroup("Events")] public UnityEvent onInComplete;
    [FoldoutGroup("Events")] public UnityEvent onOutComplete;

    [Space,FoldoutGroup("Runtime")]
    public bool state = false;

    private Tween tween;

    private float curValue;

    private void Start()
    {
        state = gameObject.activeSelf;
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
                tween = DOTween.To(() => curValue, x => SetValue(x), 1.0f, timeIn)
                  .SetUpdate(ignoreTimeScale)
                  .OnComplete(OnInComplete);
            }
            else
            {
                tween = DOTween.To(() => curValue, x => SetValue(x), 0.0f, timeOut)
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
            State = true;
        }
    }

    [FoldoutGroup("Actions"), Button("Out")]
    public override void Out()
    {

        if ( state == true )
        {
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
        gameObject.SetActive(state);
    }

    public void SetValue ( float value)
    {
        curValue = value;
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

        gameObject.SetActive(true);
    }

    private void OnOutComplete()
    {
        state = false;

        gameObject.SetActive(false);

        onOutComplete.Invoke();
    }
}
