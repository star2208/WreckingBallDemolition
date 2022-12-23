using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIHudItem : MonoBehaviour
{
    public TextMeshProUGUI text;
    public new AudioData audio;

    public List<StepInfo> stepInfos;

    public int Point
    {
        get { return point; }
        set
        {
            point = value;
            if(text) text.text = "+ " + point.ToString();
        }
    }
    private int point;

    public void In()
    {
        audio?.Play();
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        var tweenAlpha = GetComponent<TweenAlpha>();
        var tweenScale = GetComponent<TweenScale>();
        var tweenPosition = GetComponent<TweenPosition>();

        for (int i = 0; i < stepInfos.Count-1; i++)
        {

            tweenAlpha.valueOut = stepInfos[i].alpha;
            tweenAlpha.valueIn = stepInfos[i + 1].alpha;
            tweenAlpha.timeIn = stepInfos[i].time;
            tweenAlpha.easeIn = stepInfos[i].ease;
            tweenAlpha.ForceIn();

            tweenPosition.valueOut = stepInfos[i].offset;
            tweenPosition.valueIn = stepInfos[i+1].offset;
            tweenPosition.timeIn = stepInfos[i].time;
            tweenPosition.easeIn = stepInfos[i].ease;
            tweenPosition.ForceIn();

            float scaleOut = stepInfos[i].scale;
            float scaleIn = stepInfos[i + 1].scale;
            tweenScale.valueOut = new Vector3(scaleOut, scaleOut, scaleOut);
            tweenScale.valueIn = new Vector3(scaleIn, scaleIn, scaleIn);
            tweenScale.timeIn = stepInfos[i].time;
            tweenScale.easeIn = stepInfos[i].ease;
            tweenScale.ForceIn();

            yield return new WaitForSecondsRealtime(stepInfos[i].time);
        }

        GameObject.Destroy(gameObject);
    }

    [System.Serializable]
    public class StepInfo
    {
        public float alpha;
        public float scale;
        public Vector2 offset;
        public Ease ease;
        public float time;
    }
}
