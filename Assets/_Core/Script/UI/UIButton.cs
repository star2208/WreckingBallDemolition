using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

using Sirenix.OdinInspector;
using MoreMountains.NiceVibrations;

//UI 按钮
public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler,IPointerUpHandler,IPointerClickHandler, IMoveHandler
{
    //事件
    [FoldoutGroup("Unity Event")] public UnityEvent PointerEnter;
    [FoldoutGroup("Unity Event")] public UnityEvent PointerExit;
    [FoldoutGroup("Unity Event")] public UnityEvent PointerDown;
    [FoldoutGroup("Unity Event")] public UnityEvent PointerUp;
    [FoldoutGroup("Unity Event")] public UnityEvent PointerClick;
    [FoldoutGroup("Unity Event")] public UnityEvent PointerClickDouble;
    [FoldoutGroup("Unity Event")] public UnityEvent PointerMove;

    [FoldoutGroup("Event")] public string eventName;

    [FoldoutGroup("Params")] public AudioData audioClick;                        //按下的音效
    [FoldoutGroup("Params")] public HapticTypes hapticType = HapticTypes.None;   //震动
    [FoldoutGroup("Params")] public KeyCode altKey = KeyCode.None;               //替代的键

    public virtual void OnPointerEnter( PointerEventData d)
    {
        PointerEnter.Invoke();
    }

    public virtual void OnPointerExit(PointerEventData d)
    {
        PointerExit.Invoke();
    }

    public virtual void OnPointerDown(PointerEventData d)
    {
        HapticManager.Instance.Trigger(hapticType);
        audioClick?.Play();
        PointerDown.Invoke();
    }

    public virtual void OnPointerUp(PointerEventData d)
    {
        PointerUp.Invoke();
    }

    public virtual void OnPointerClick(PointerEventData d)
    {
        PointerClick.Invoke();

        if (!string.IsNullOrEmpty(eventName)) EventManager.EmitEvent(eventName);

        if( d.clickCount== 2 ) PointerClickDouble.Invoke();

        //if ( audioClick) AudioCenter.Instance.Play(audioClick);
    }

    public virtual void OnMove ( AxisEventData d)
    {
        PointerMove.Invoke();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(altKey))
        //{
        //    OnPointerDown(null);
        //}

        //if (Input.GetKeyUp(altKey))
        //{
        //    OnPointerUp(null);
        //}
    }
}
