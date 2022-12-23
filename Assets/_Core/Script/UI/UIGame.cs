using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class UIGame : MonoBehaviour
{
    static public UIGame instance;

    [FoldoutGroup("Object")] public RectTransform areaAdapter;                     //适配区域
    [Space]
    [FoldoutGroup("Object")] public UIPage pageMenu;
    [FoldoutGroup("Object")] public UIPage pageOptions;
    [FoldoutGroup("Object")] public UIPage pageLevels;
    [FoldoutGroup("Object")] public UIPage pageShop;
    [FoldoutGroup("Object")] public UIPage pageGame;
    [FoldoutGroup("Object")] public UIPage pagePause;
    [FoldoutGroup("Object")] public UIPage pageFailed;
    [FoldoutGroup("Object")] public UIPage pageSucceeded;
    [FoldoutGroup("Object")] public UIPage pageAdFailed;
    [Space]
    [FoldoutGroup("Object")] public UIPopTip popTipNotification;
    [FoldoutGroup("Object")] public UIPopTip popTipGeneral;
    [Space]
    [FoldoutGroup("Object")] public UIHudArea areaHud;
    [FoldoutGroup("Object")] public UIHealthBarArea areaHealthBar;

    [FoldoutGroup("Adapter")] public Vector2 adapterOffesetCommon;
    [FoldoutGroup("Adapter")] public Vector2 adapterOffesetNotch;


    public delegate void SimpleButtonDelegate ();

    private void Awake()
    {
        instance = this;
    }


    private void Start()
    {
    }


    //世界坐标转 UI 空间坐标 anchor 在中心的
    public Vector2 WorldToCanvas(Vector3 worldPos)
    {

        var viewportPos = CameraManager.instance.main.WorldToViewportPoint(worldPos);
        var canvasRect = GetComponent<RectTransform>();

        var uiPos = new Vector2((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                            (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));

        return uiPos;
    }

}
