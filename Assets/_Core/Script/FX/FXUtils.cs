using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;
using UnityEngine.UI;

//外观 工具
public class FXUtils : MonoBehaviour
{
    //播放控制
    [ToggleGroup("enablePlaybackControl","Playback Control")] public bool enablePlaybackControl; 
    [ToggleGroup("enablePlaybackControl")] public List<GameObject> gameObjects;                     //受播放控制的对象
    [ToggleGroup("enablePlaybackControl")] public List<ParticleSystem> particleSystems;             //受播放控制的粒子系统
    [ToggleGroup("enablePlaybackControl")] public bool isPlaying = true;
    public bool IsPlaying
    {
        get { return isPlaying; }
        set
        {
            if (isPlaying == value) return;
            isPlaying = value;

            if (isPlaying == true)
            {
                foreach (GameObject go in gameObjects)
                {
                    go.SetActive(true);
                }
                ParticleSystem.EmissionModule e;
                foreach (ParticleSystem ps in particleSystems)
                {
                    e = ps.emission;
                    e.enabled = true;
                }
            }
            else
            {
                if (gameObjects != null)
                {
                    foreach (GameObject go in gameObjects)
                    {
                        go.SetActive(false);
                    }
                }
                if (particleSystems != null)
                {
                    ParticleSystem.EmissionModule e;
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        e = ps.emission;
                        e.enabled = false;
                    }
                }
            }
        }
    }

    //自动销毁
    [ToggleGroup("enableAutoDestroy", "Auto Destroy")] public bool enableAutoDestroy;
    [ToggleGroup("enableAutoDestroy")] public float autoDestroyTime;                            //自动销毁时间
    [ToggleGroup("enableAutoDestroy")] public float autoDestroyDelay;                           //自动销毁迟延
    [HideInInspector] private float autoDestroyTimer = 0.0f;

    //自动开启 需配合播放控制
    [ToggleGroup("enableAutoEnable", "Auto Enable")] public bool enableAutoEnable;
    [ToggleGroup("enableAutoEnable")] public float autoEnableTime;
    [HideInInspector] private float autoEnableTimer = 0.0f;

    //旋转
    //[ToggleGroup("enableRotation", "Rotation")] public bool enableRotation;
    //[ToggleGroup("enableRotation")] public Vector3 rotationSpeed;

    //缩放曲线
    //[ToggleGroup("enableScaleCurve", "Scale Curve")] public bool enableScaleCurve;
    //[ToggleGroup("enableScaleCurve")] public AnimationCurve scaleCurve;              //动画曲线
    //[ToggleGroup("enableScaleCurve")] public float scaleCurveDuration;               //周期时间
    //[ToggleGroup("enableScaleCurve")] public bool scaleCurveLoop = true;             //是否循环
    //[ToggleGroup("enableScaleCurve")] public bool scaleCurveRandomStartPos = false;  //是否随机开始位置
    //[HideInInspector] private float scaleCurveTimer = 0.0f;                          //计时器


    //Alpha 曲线
    //[ToggleGroup("enableAlphaCurve", "Alpha Curve")] public bool enableAlphaCurve;
    //[ToggleGroup("enableAlphaCurve")] public AnimationCurve alphaCurve;                           //动画曲线
    //[ToggleGroup("enableAlphaCurve")] public float alphaCurveDuration;                            //周期时间
    //[ToggleGroup("enableAlphaCurve")] public bool alphaCurveLoop = true;                          //是否循环
    //[ToggleGroup("enableAlphaCurve")] public bool alphaCurveMutiplier = true;                     //是否乘上初始值
    //[HideInInspector] private float alphaCurveTimer = 0.0f;                                       //计时器
    //[HideInInspector] private Color curColor;                                                     //当前颜色
    //[HideInInspector] private float alphaCurveOriginValue;                                        //初始值
    //[HideInInspector] SpriteRenderer alphaCurve_SpriteRenderer;
    //[HideInInspector] Light alphaCurve_Light;
    //[HideInInspector] Image alphaCurve_Image;

    private void Start ()
    {
        ////缩放曲线
        //if (enableScaleCurve)
        //{
        //    if (scaleCurveRandomStartPos) scaleCurveTimer = Random.value;

        //    float scale = scaleCurve.Evaluate(scaleCurveTimer);
        //    transform.localScale = new Vector3(scale, scale, scale);
        //}

        ////Alpha 曲线
        //if (enableAlphaCurve)
        //{
        //    //获取应用 alpha 曲线的对象
        //    alphaCurve_SpriteRenderer = GetComponent<SpriteRenderer>();
        //    alphaCurve_Image = GetComponent<Image>();
        //    alphaCurve_Light = GetComponent<Light>();

        //    float alpha = alphaCurve.Evaluate(0.0f);
        //    if (alphaCurve_SpriteRenderer)
        //    {
        //        alphaCurveOriginValue = alphaCurve_SpriteRenderer.color.a;

        //        curColor = alphaCurve_SpriteRenderer.color;
        //        curColor.a = alpha * (alphaCurveMutiplier ? alphaCurveOriginValue : 1.0f);
        //        alphaCurve_SpriteRenderer.color = curColor;
        //    }

        //    if (alphaCurve_Image)
        //    {
        //        alphaCurveOriginValue = alphaCurve_Image.color.a;

        //        curColor = alphaCurve_Image.color;
        //        curColor.a = alpha * (alphaCurveMutiplier ? alphaCurveOriginValue : 1.0f);
        //        alphaCurve_Image.color = curColor;
        //    }

        //    if (alphaCurve_Light)
        //    {
        //        alphaCurveOriginValue = alphaCurve_Light.intensity;

        //        alphaCurve_Light.intensity = alpha * (alphaCurveMutiplier ? alphaCurveOriginValue : 1.0f);
        //    }
        //}
    }

    private void Update()
    {
        //自动销毁
        if ( enableAutoDestroy && autoDestroyTime > 0.0f )
        {
            autoDestroyTimer += Time.deltaTime;
            if (autoDestroyTimer >= autoDestroyTime)
            {
                Destroy();
                enableAutoDestroy = false;
            }
        }

        //自动开启
        if (enableAutoEnable && autoEnableTime > 0.0f)
        {
            autoEnableTimer += Time.deltaTime;
            if (autoEnableTimer >= autoEnableTime)
            {
                IsPlaying = true;
                enableAutoEnable = false;
            }
        }

        ////旋转
        //if (enableRotation)
        //{
        //    transform.Rotate(rotationSpeed * Time.deltaTime);
        //}

        ////缩放曲线
        //if (enableScaleCurve)
        //{
        //    scaleCurveTimer += Time.deltaTime;
        //    if (scaleCurveTimer >= scaleCurveDuration)
        //    {
        //        if (scaleCurveLoop == false)
        //        {
        //            enableScaleCurve = false;
        //            scaleCurveTimer = scaleCurveDuration;
        //        }
        //        else scaleCurveTimer = 0.0f;
        //    }

        //    float scale = scaleCurve.Evaluate(scaleCurveTimer / scaleCurveDuration);
        //    transform.localScale = new Vector3(scale, scale, scale);
        //}

        ////Alpha 曲线
        //if (enableAlphaCurve)
        //{
        //    alphaCurveTimer += Time.deltaTime;
        //    if (alphaCurveTimer >= alphaCurveDuration)
        //    {
        //        if (alphaCurveLoop == false)
        //        {
        //            enableAlphaCurve = false;
        //            alphaCurveTimer = alphaCurveDuration;
        //        }
        //        else alphaCurveTimer = 0.0f;
        //    }

        //    float alpha = alphaCurve.Evaluate(alphaCurveTimer / alphaCurveDuration);

        //    if (alphaCurve_SpriteRenderer)
        //    {
        //        curColor = alphaCurve_SpriteRenderer.color;
        //        curColor.a = alpha * (alphaCurveMutiplier ? alphaCurveOriginValue : 1.0f );
        //        alphaCurve_SpriteRenderer.color = curColor;
        //    }

        //    if (alphaCurve_Image)
        //    {
        //        curColor = alphaCurve_Image.color;
        //        curColor.a = alpha * (alphaCurveMutiplier ? alphaCurveOriginValue : 1.0f);
        //        alphaCurve_Image.color = curColor;
        //    }

        //    if (alphaCurve_Light)
        //    {
        //        alphaCurve_Light.intensity = alpha * (alphaCurveMutiplier ? alphaCurveOriginValue : 1.0f);
        //    }
        //}
    }

    //手动销毁
    public void Destroy()
    {
        IsPlaying = false;
        GameObject.Destroy(gameObject, autoDestroyDelay);
    }

    public void PlayOnce()
    {
        foreach (ParticleSystem ps in particleSystems)
        {
            ps.Play();
        }
    }

}
