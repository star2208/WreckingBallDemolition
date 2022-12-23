using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXRotationCurve : MonoBehaviour
{
    public AnimationCurve curve;                                                        //曲线
    public float duration = 1.0f;                                                       //周期时间
    public bool loop = false;                                                           //是否循环
    public bool x;
    public bool y;
    public bool z;

    private float timer = 0.0f;                                                         //计时器
    private float curValue;                                                             //当前值
    private Vector3 originValue;                                                        //初始值

    private void Awake()
    {
        if ( transform )
        {
            originValue = transform.localRotation.eulerAngles;
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
        Vector3 value = originValue;
        if (x) value.x = curValue;
        if (y) value.y = curValue;
        if (z) value.z = curValue;

        if (transform)
        {

            transform.localRotation = Quaternion.Euler(value);
            return;
        }

    }
}
