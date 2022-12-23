using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine.UI;

public class UIPopTipItem: MonoBehaviour
{
    [FoldoutGroup("Objects")] public TextMeshProUGUI text;

    [FoldoutGroup("Params")] public AudioData audioIn;

    [FoldoutGroup("Runtime")] public int step = 0;          //当前阶段
    [FoldoutGroup("Runtime")] public bool isActive;

    private UIPopTip popTip;
    private TweenAlpha tweenAlpha;
    private TweenScale tweenScale;
    private TweenPosition tweenPosition;
    private float stayTimer;

    //值
    public virtual object Value
    {
        get { return value; }
        set
        {
            this.value = value;
            UpdateContent();
        }
    }
    protected object value;

    //类型
    public virtual PopTipType Type
    {
        get { return type; }
        set
        {
            type = value;
            UpdateContent();
        }
    }
    private PopTipType type;

    //根据值和类型设置内容
    public void UpdateContent()
    {
        //通用文字
        if ( type == PopTipType.PureText )
        {
            text.text =
                value.GetType().GetProperty("text")?.GetValue(value).ToString();
        }
        //关卡开始
        else if (type == PopTipType.Level)
        {

            int level = (int)value.GetType().GetProperty("level").GetValue(value);
            text.text =
                "LEVEL " + level.ToString() + " STARTS";
        }
    }


    public void Step()
    {
        int nextStep = step + 1;

        tweenAlpha.valueOut = popTip.stepInfos[step].alpha;
        tweenAlpha.valueIn = popTip.stepInfos[nextStep].alpha;
        tweenAlpha.timeIn = popTip.timeStep;
        tweenAlpha.ForceIn();

        tweenPosition.valueOut = popTip.stepInfos[step].offset;
        tweenPosition.valueIn = popTip.stepInfos[nextStep].offset;
        tweenPosition.timeIn = popTip.timeStep;
        tweenPosition.ForceIn();

        float scaleOut = popTip.stepInfos[step].scale;
        float scaleIn = popTip.stepInfos[nextStep].scale;
        tweenScale.valueOut = new Vector3(scaleOut, scaleOut, scaleOut);
        tweenScale.valueIn = new Vector3(scaleIn, scaleIn, scaleIn);
        tweenScale.timeIn = popTip.timeStep;
        tweenScale.ForceIn();

        step = nextStep;
        stayTimer = 0.0f;
    }
    public void Out()
    {
        tweenAlpha.valueIn = popTip.stepInfos[step].alpha;
        tweenAlpha.valueOut = 0.0f;
        tweenAlpha.timeOut = popTip.timeStep;
        tweenAlpha.ForceOut();


        float scaleIn = popTip.stepInfos[step].scale;
        float scaleOut = popTip.stepInfos[step].scale * 0.5f;
        tweenScale.valueIn = new Vector3(scaleIn, scaleIn, scaleIn);
        tweenScale.valueOut = new Vector3(scaleOut, scaleOut, scaleOut);
        tweenScale.timeOut = popTip.timeStep;
        tweenScale.ForceOut();

        step = popTip.stepInfos.Count - 2;
    }
    public void Reset()
    {
        tweenAlpha.valueOut = popTip.stepInfos[0].alpha;
        tweenAlpha.ForceSetState(false);

        tweenPosition.valueOut = popTip.stepInfos[0].offset;
        tweenPosition.ForceSetState(false);

        tweenScale.valueOut = Vector3.zero;
        tweenScale.ForceSetState(false);

        step = 0;
    }
    private void Awake()
    {
        tweenAlpha = GetComponent<TweenAlpha>();
        tweenScale = GetComponent<TweenScale>();
        tweenPosition = GetComponent<TweenPosition>();

        popTip = transform.parent.GetComponent<UIPopTip>();
    }

    private void Update()
    {
        if (isActive)
        {
            stayTimer += Time.deltaTime;
            if (stayTimer > popTip.stepInfos[step].stayTime)
            {
                Out();
                isActive = false;
                popTip.idleItems.Enqueue(popTip.activeItems.Dequeue());
            }
        }
    }
}
