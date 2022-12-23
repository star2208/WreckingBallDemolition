using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering;

using Cinemachine;
using Sirenix.OdinInspector;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [FoldoutGroup("Object")] public Camera main;
    [FoldoutGroup("Object")] public Volume postProcessVolume;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
    }

    private bool isShake = false;

    public void CameraShake()
    {
        if (isShake)
            return;

        isShake = true;
        transform.DOShakePosition(0.8f, 2f).OnComplete(() =>
        {
            //transform.position = cameraPos.position;
            //transform.rotation = cameraPos.rotation;
            isShake = false;
        });
    }

    public void MoveToNextBuilding()
    {
        transform.DOMoveX(GameManager.instance.curBuilding.transform.position.x, 1.5f);
    }
}
