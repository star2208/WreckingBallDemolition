using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHealthBarArea : MonoBehaviour
{
    public GameObject healthBarPrefab;

    //添加一个血条
    //参数 目标对象 偏移 类型
    public UIHealthBarHUD AddHealthBar ( Transform target , Vector3 offset )
    {
        var prefab = healthBarPrefab;

        var bar = Instantiate(prefab, transform).GetComponent<UIHealthBarHUD>();

        var anchor = bar.GetComponent<UISceneAnchor>();
        anchor.target = target;
        anchor.sceneOffset = offset;

        return bar;
    }
}
