using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using TMPro;

public class UILifeCounter : MonoBehaviour
{
    [FoldoutGroup("Object")] public TextMeshProUGUI text;
    [FoldoutGroup("Object")] public TweenScale icon;

    [FoldoutGroup("Runtime"), ShowInInspector]
    public int Count
    {
        get { return count; }
        set
        {
            if (value < count) icon.ForceIn();

            count = value;
            text.text = count.ToString();
        }
    }
    private int count;
}
