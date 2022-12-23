using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Slider))]
public class UIHealthBar : MonoBehaviour
{
    [FoldoutGroup("Object")] public Image marker;
    [FoldoutGroup("Object")] public Slider sliderWhite;

    private Slider Slider
    {
        get
        {
            if (slider == null) slider = GetComponent<Slider>();
            return slider;
        }
    }
    private Slider slider;

    private TweenScale Marker_TweenScale
    {
        get
        {
            if (marker_tweenScale == null ) marker_tweenScale = marker.GetComponent<TweenScale>();
            return marker_tweenScale;
        }
    }
    private TweenScale marker_tweenScale;

    private TweenColor Marker_TweenColor
    {
        get
        {
            if (marker_tweenColor == null) marker_tweenColor = marker.GetComponent<TweenColor>();
            return marker_tweenColor;
        }
    }
    private TweenColor marker_tweenColor;

    private TweenAlpha SliderWhite_TweenAlpha
    {
        get
        {
            if (sliderWhite_tweenAlpha == null) sliderWhite_tweenAlpha = sliderWhite.GetComponent<TweenAlpha>();
            return sliderWhite_tweenAlpha;
        }
    }
    private TweenAlpha sliderWhite_tweenAlpha;

    [FoldoutGroup("Runtime"), ShowInInspector]
    public float Value
    {
        get { return value; }
        set
        {
            if (sliderWhite) sliderWhite.value = this.value;
            SliderWhite_TweenAlpha?.ForceIn();

            if (value < this.value)
            {
                Marker_TweenScale.ForceIn();
                Marker_TweenColor.ForceIn();
                SliderWhite_TweenAlpha?.ForceIn();
            }

            this.value = Mathf.Clamp01(value);
            Slider.value = value;
        }
    }
    private float value = 1.0f;
}
