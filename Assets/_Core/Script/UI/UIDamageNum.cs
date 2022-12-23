using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDamageNum : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Vector2 radius;
    public Vector2 emitSpeed;
    public float drag;

    private RectTransform rectTransform;
    private Vector2 uiPos;
    private Vector2 curSpeedEmit;

    public void Set ( int damage, Vector3 worldPos, bool critical )
    {
        text.text = damage.ToString();

        uiPos = UIGame.instance.WorldToCanvas(worldPos);
        uiPos += Random.insideUnitCircle.normalized * radius.Random();

        curSpeedEmit = Random.insideUnitCircle.normalized * emitSpeed.Random();

        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = uiPos;

        if ( critical)
        {
            text.color = new Color(1.0f, 0.75f, 0.15f);
        }
    }

    private void Update()
    {
        curSpeedEmit = Vector2.Lerp(curSpeedEmit, Vector2.zero, drag * Time.deltaTime);
        uiPos += curSpeedEmit * Time.deltaTime;
        rectTransform.anchoredPosition = uiPos;
    }
}
