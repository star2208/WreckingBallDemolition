using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using DG.Tweening;

public class TweenMaterial : MonoBehaviour
{
    [FoldoutGroup("Params")] public Material valueIn;
    [FoldoutGroup("Params")] public Material valueOut;

    [FoldoutGroup("Params")] public bool startState = true;

    private Renderer Renderer
    {
        get
        {
            if (renderer == null) renderer = GetComponent<MeshRenderer>();
            return renderer;
        }
    }
    private new Renderer renderer;

    [Space,FoldoutGroup("Params")]
    public bool state = false;

    private Tween tween;

    private void Start()
    {
        state = startState;

        if ( startState == true)
        {
            if (state == true) Renderer.material = valueIn;
        }
        else
        {
            if(state == false) Renderer.material = valueOut;
        }
    }

    public bool State
    {
        get
        {
            return state;
        }
        set
        {
            if ( state == value) return;
            state = value;

            if (state == true)
            {
                Renderer.material = valueIn;
            }
            else
            {
                Renderer.material = valueOut;
            }
        }
    }

    [FoldoutGroup("Actions"), Button("In")]
    public void In()
    {
        if ( state == false )
        {
            State = true;
        }
    }

    [FoldoutGroup("Actions"), Button("Out")]
    public void Out()
    {

        if ( state == true )
        {
            State = false;
        }
    }

    public void ForceIn()
    {
        ForceSetState(false);
        In();
    }

    public void ForceOut()
    {
        ForceSetState(true);
        Out();
    }

    //强制设置状态
    public void ForceSetState( bool state)
    {
        this.state = state;
        Renderer.material = state ? valueIn : valueOut;
    }

    public void Switch()
    {
        if ( state == true)
        {
            Out();
        }
        else
        {
            In();
        }
    }
}
