using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using MoreMountains.NiceVibrations;

public class UIClickCircleArea : MonoBehaviour, IPointerDownHandler
{
    public UIClickCircle clickCircle;
    public HapticTypes hapticType;

    private RectTransform rectTransform;

    public void OnPointerDown(PointerEventData d)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, d.position, null, out Vector2 localPos);
        clickCircle.transform.localPosition = localPos;
        clickCircle.Show();

        HapticManager.Instance.Trigger(hapticType);
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }
}
