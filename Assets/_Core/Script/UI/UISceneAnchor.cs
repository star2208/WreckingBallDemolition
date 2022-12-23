using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class UISceneAnchor : MonoBehaviour
{
    public Transform target;

    public Vector3 sceneOffset;
    public Vector2 uiOffset;

    private Vector2 uiPos;
    

    private RectTransform RectTransform
    {
        get
        {
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }
            return rectTransform;
        }
    }
    private RectTransform rectTransform;


    private void Update()
    {
        if ( target)
        {
            UpdatePos();
        }
    }


    private void UpdatePos()
    {
        uiPos = UIGame.instance.WorldToCanvas(target.position + sceneOffset);
        uiPos += uiOffset;

        RectTransform.anchoredPosition = uiPos;
    }
}
