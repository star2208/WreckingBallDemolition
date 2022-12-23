using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FXAlphaCurve : MonoBehaviour
{
    public AnimationCurve curve;                                                        //曲线
    public float duration = 1.0f;                                                       //周期时间
    public bool loop = false;                                                           //是否循环
    public bool mutiplier = true;                                                       //是否乘上初始值
    public bool destroyWhenComplete = false;                                            //是否在完成后销毁 对循环的无效

    private float timer = 0.0f;                                                         //计时器
    private float curValue;                                                             //当前值
    private float originValue;                                                          //初始值

    private CanvasGroup canvasGrounp;
    private Image image;

    private void Awake()
    {
        canvasGrounp = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();

        if (canvasGrounp)
        {
            originValue = canvasGrounp.alpha;
        }
        if ( image)
        {
            originValue = image.color.a;
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
            if (loop == false)
            {
                timer = duration;
                if (destroyWhenComplete) Destroy(gameObject);
            }

            else timer = 0.0f;
        }
        else timer += Time.deltaTime;

        curValue = curve.Evaluate(timer / duration);
        UpdateValue();
    }

    private void UpdateValue()
    {
        float value = curValue * (mutiplier ? originValue : 1.0f);

        if (canvasGrounp)
        {
            canvasGrounp.alpha = value;
        }
        if ( image)
        {
            var c = image.color;
            c.a = value;
            image.color = c;
        }

    }
}
