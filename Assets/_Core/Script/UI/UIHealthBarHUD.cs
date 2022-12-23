using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

//跟随场景物体的生命条
[RequireComponent(typeof(Slider))]
public class UIHealthBarHUD : UIHealthBar
{
    [FoldoutGroup("Params")] public float showTime = 2.0f;                  //显示后再隐藏的时间 设为 0 禁用

    private TweenAlpha tweenAlpha;
    private float showTimer;

    //是否显示
    [FoldoutGroup("Runtime"), ShowInInspector]
    public bool IsShowed
    {
        get { return isShowed; }
        set
        {
            showTimer = 0.0f;
            isShowed = value;
            if (tweenAlpha == null) tweenAlpha = GetComponent<TweenAlpha>();
            tweenAlpha.State = value;
        }

    }
    private bool isShowed;

    //长度
    [FoldoutGroup("Runtime"), ShowInInspector]
    public float Width
    {
        get { return width; }
        set
        {
            width = value;
            var size = GetComponent<RectTransform>().sizeDelta;
            size.x = width;
            GetComponent<RectTransform>().sizeDelta = size;
        }
    }
    private float width = 200.0f;

    private void Update()
    {
        if ( isShowed == true && showTime > 0.0f )
        {
            showTimer += Time.deltaTime;
            if ( showTimer > showTime)
            {
                showTimer = 0.0f;
                IsShowed = false;
            }
        }
    }
}
