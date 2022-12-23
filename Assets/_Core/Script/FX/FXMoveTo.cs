using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Sirenix.OdinInspector;

public class FXMoveTo : MonoBehaviour
{
    [FoldoutGroup("Params")] public MoveType moveType;

    [FoldoutGroup("Params"), ShowIf("Editor_IsLinear")] public float linearSpeed = 1.0f;
    [FoldoutGroup("Params"), ShowIf("Editor_IsLerp")] public float lerpSpeed = 1.0f;
    [FoldoutGroup("Params"), ShowIf("Editor_IsBezier")] public Vector3 bezierOffset = new Vector3(0.0f, 1.0f, 0.0f); //bezier 曲线中点相对起始位置中点的偏移
    [FoldoutGroup("Params"), ShowIf("Editor_IsBezier")] public float bezierSpeed = 1.0f;

    [FoldoutGroup("Params")] public float threshold = 0.1f;
    [Space]
    [FoldoutGroup("Params")] public bool enableScaleCurve;
    [FoldoutGroup("Params")] public AnimationCurve scaleCurve;

    [FoldoutGroup("Event")] public UnityEvent onReachTarget;

    [FoldoutGroup("Runtime")] public Transform target;
    [FoldoutGroup("Runtime")] public Vector3 targetPos;

    private bool reached;
    private float startDistance;
    private Vector3 startPos;
    private Vector3 p;
    private Vector3 m;
    private float d;
    private float t;
    private float s;
    private float bezier_t = 0.0f;

    private void OnEnable()
    {
        reached = false;
        bezier_t = 0.0f;

        if (target) p = target.transform.position;
        else p = targetPos;

        startPos = transform.position;
        startDistance = Vector3.Distance(startPos, p);
    }

    private void Update()
    {
        if (reached) return;

        //更新目标位置
        if (target) p = target.transform.position;
        else p = targetPos;

        //移动
        if (moveType == MoveType.Linear)
        {
            transform.position = Vector3.MoveTowards(transform.position, p, linearSpeed * Time.deltaTime);
        }
        else if ( moveType == MoveType.Lerp)
        {
            transform.position = Vector3.Lerp(transform.position, p, lerpSpeed * Time.deltaTime);
        }
        else if (moveType == MoveType.Bezier)
        {
            bezier_t += bezierSpeed * Time.deltaTime;
            m = (startPos + p) * 0.5f;
            transform.position = Bezier.Quad(startPos, m + bezierOffset, p, bezier_t);
        }

        //距离和缩放曲线
        d = Vector3.Distance(transform.position, p);
        if ( enableScaleCurve)
        {
            t = 1.0f - Mathf.Clamp01(d / startDistance);
            s = scaleCurve.Evaluate(t);
            transform.localScale = Vector3.one * s;
        }

        //是否到达目标
        if ( d <= threshold)
        {
            reached = true;
            this.enabled = false;
            if ( onReachTarget != null ) onReachTarget.Invoke();
        }
    }

    public enum MoveType
    {
        Linear,
        Lerp,
        Bezier
    }

#if UNITY_EDITOR
    private bool Editor_IsLinear
    {
        get { return moveType == MoveType.Linear; }
    }

    private bool Editor_IsLerp
    {
        get { return moveType == MoveType.Lerp; }
    }

    private bool Editor_IsBezier
    {
        get { return moveType == MoveType.Bezier; }
    }
#endif
}
