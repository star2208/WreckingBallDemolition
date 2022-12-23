using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


//用于画圆的, 比起用 Sprite 可以固定线的粗细
[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class FXLineRendererCircle : MonoBehaviour
{
    //总顶点数
    [FoldoutGroup("Params"), ShowInInspector]                  
    public int VertexCount
    {
        get { return vertexCount; }
        set
        {
            vertexCount = value;
            UpdateLineRenderer();
        }
    }
    [SerializeField, HideInInspector]
    private int vertexCount = 40;

    //线粗细
    [FoldoutGroup("Params"), ShowInInspector]
    public float LineWitdh
    {
        get { return lineWidth; }
        set
        {
            lineWidth = value;
            UpdateLineRenderer();
        }
    }
    [SerializeField, HideInInspector]
    private float lineWidth = 0.2f;


    //半径
    [FoldoutGroup("Params"), ShowInInspector]
    public float Radius
    {
        get { return radius; }
        set
        {
            radius = value;
            UpdateLineRenderer();
        }
    }
    [SerializeField, HideInInspector]
    private float radius = 2.0f;


    private LineRenderer LineRenderer
    {
        get
        {
            if (lineRenderer == null)
            {
                lineRenderer = GetComponent<LineRenderer>();
            }
            return lineRenderer;
        }
    }
    private LineRenderer lineRenderer;

    private void UpdateLineRenderer()
    {
        LineRenderer.widthMultiplier = lineWidth;

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        LineRenderer.positionCount = vertexCount;
        for (int i = 0; i < LineRenderer.positionCount; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
            LineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    private void OnEnable()
    {
        LineRenderer.loop = true;
        LineRenderer.useWorldSpace = false;
        UpdateLineRenderer();
    }
}