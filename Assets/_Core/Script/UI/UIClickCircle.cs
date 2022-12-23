using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UIClickCircle : MonoBehaviour
{
    public TweenAlpha tweenAlpha;
    public TweenScale tweenScale;

    [Button("Show")]
    public void Show()
    {
        tweenAlpha.ForceIn();
        tweenScale.ForceIn();
    }
}
