using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXIntensityCurve : MonoBehaviour
{
    public AnimationCurve curve;                                                        //曲线
    public float duration = 1.0f;                                                       //周期时间
    public bool loop = false;                                                            //是否循环
    public bool mutiplier = true;                                                       //是否乘上初始值

    private float timer = 0.0f;                                                         //计时器
    private float curValue;                                                             //当前值
    private float originValue;                                                          //初始值

    private new Light light;

    private void Awake()
    {
        light = GetComponent<Light>();

        if (light)
        {
            originValue = light.intensity;
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
        float value = curValue * (mutiplier ? originValue : 1.0f);
        if (light)
        {
            light.intensity = value;
            return;
        }

    }
}
