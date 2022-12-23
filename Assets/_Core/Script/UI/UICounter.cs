using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Sirenix.OdinInspector;
using TMPro;

//资源计数 UI
public class UICounter : MonoBehaviour
{
    [FoldoutGroup("Object")] public Image icon;
    [FoldoutGroup("Object")] public TextMeshProUGUI text;

    [FoldoutGroup("Audio")] public AudioData audioAdd;

    private TweenScale tweenScale_up;
    private TweenScale tweenScale_Down;

    [FoldoutGroup("Runtime"), ShowInInspector]
    public int Count
    {
        get { return count; }
        set
        {
            if ( Application.isPlaying)
            {
                if (value > count)
                {
                    tweenScale_up.ForceIn();
                    if(audioAdd) audioAdd.Play();
                }
                if (value < count) tweenScale_Down.ForceIn();
            }

            count = value;
            text.text = count.Comma();
        }
    }
    private int count;

    private void Awake()
    {
        tweenScale_up = text.GetComponents<TweenScale>()[0];
        tweenScale_Down = text.GetComponents<TweenScale>()[1];
    }
}
