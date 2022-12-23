using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHudArea : MonoBehaviour
{
    //public GameObject addPointPrefab;
    //public GameObject heartBreakPrefab;
    //public GameObject heartPrefab;

    public List<PrefabInfo> prefabInfos;

    public enum Type
    {
        AddPoint
    }

    public void Add( int value, Vector2 worldPos, Type type = Type.AddPoint)
    {

        Vector2 uiPos = UIGame.instance.WorldToCanvas(worldPos);

        var prefab = prefabInfos.Find(x => x.type == type).prefab;

        if ( prefab != null)
        {
            UIHudItem hudItem = GameObject.Instantiate(prefab, transform).GetComponent<UIHudItem>();
            hudItem.Point = value;
            hudItem.GetComponent<RectTransform>().anchoredPosition = uiPos;
            hudItem.In();
        }
    }

    [System.Serializable]
    public class PrefabInfo
    {
        public Type type;
        public GameObject prefab;
    }
}
