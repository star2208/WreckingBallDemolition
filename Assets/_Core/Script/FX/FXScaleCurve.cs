using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXScaleCurve : MonoBehaviour
{
    public AnimationCurve curve;                                                        //曲线
    public float duration = 1.0f;                                                       //周期时间
    public bool loop = false;                                                           //是否循环
    public bool mutiplier = true;                                                       //是否乘上初始值

    private float timer = 0.0f;                                                         //计时器
    private float curValue;                                                             //当前值
    private Vector3 originValue;                                                        //初始值

    private void Awake()
    {
        if ( transform )
        {
            originValue = transform.localScale;
            return;
        }
    }

    private void Start()
    {
        curValue = curve.Evaluate(0.0f);
        UpdateValue();
    }

    private void Update()
    {
        if (timer >= duration)
        {
            if (loop == false) timer = duration;
            else timer = 0.0f;
        }
        else timer += Time.deltaTime;

        curValue = curve.Evaluate(timer / duration);
        UpdateValue();
    }

    private void UpdateValue()
    {
        Vector3 value = curValue * (mutiplier ? originValue : Vector3.one);
        if (transform)
        {
            transform.localScale = value;
            return;
        }

    }
}
