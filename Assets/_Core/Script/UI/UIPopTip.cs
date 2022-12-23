using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;

public class UIPopTip : MonoBehaviour
{
    [FoldoutGroup("Animation")] public float addTime;                           //添加新的间隔时间
    [FoldoutGroup("Animation")] public float stayTime;                          //现有项存留的时间
    [FoldoutGroup("Animation")] public float timeStep;                          //移步动画的时间
    [FoldoutGroup("Animation")] public List<StepInfo> stepInfos;

    [FoldoutGroup("Prefab")] public GameObject popTipItemPrefab;

    private float addTimer;                         
    private RectTransform rectTransform;

    public class Info
    {
        public PopTipType type;
        public object value;
    }
    private Queue<Info> infos = new Queue<Info>();                              //待显示的列表

    public Queue<UIPopTipItem> activeItems = new Queue<UIPopTipItem>();        //现役的项
    public Queue<UIPopTipItem> idleItems = new Queue<UIPopTipItem>();          //空闲的项

    //添加一个待显示项
    [FoldoutGroup("Runtime"), Button("Add")]
    public void Add( PopTipType type, object value )
    {
        Info info = new Info
        {
            value = value,
            type = type
        };

        infos.Enqueue(info);
    }

    [FoldoutGroup("Runtime"), Button("Add Pure Text")]
    public void AddPureText ( string text = "TEXT" )
    {
        Info info = new Info
        {
            value = new { text },
            type = PopTipType.PureText
        };

        infos.Enqueue(info);
    }



    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //添加新的项
        if (infos.Count > 0)
        {
            addTimer += Time.deltaTime;
            if (addTimer >= addTime)
            {
                addTimer = 0.0f;
                Info info = infos.Dequeue();

                UIPopTipItem item = null;
                if (idleItems.Count > 0)
                {
                    item = idleItems.Dequeue();
                }
                else
                {
                    var prefab = popTipItemPrefab;
                    if ( prefab ) item = GameObject.Instantiate( prefab, transform).GetComponent<UIPopTipItem>();
                }

                //弹出音效
                item.audioIn?.Play();

                //设置项的信息
                if (item != null)
                {
                    item.Value = info.value;
                    item.Type = info.type;
                    item.Reset();
                }

                item.isActive = true;
                activeItems.Enqueue(item);
                MoveAll();
            }
        }
    }
    private void MoveAll()
    {
        foreach ( UIPopTipItem item in activeItems)
        {
            item.Step();
        }

        //回收末尾项
        if (activeItems.Peek().step == stepInfos.Count - 1)
        {
            var item = activeItems.Dequeue();
            item.isActive = false;
            idleItems.Enqueue(item);
        }
    }

    //清空
    public void Clear()
    {
        foreach (var item in activeItems)
        {
            Destroy(item.gameObject);
        }
        activeItems.Clear();
        infos.Clear();
    }

    //辅助类
    //阶段信息
    [System.Serializable]
    public class StepInfo
    {
        public float alpha;
        public float scale;
        public Vector2 offset;
        public float stayTime;          //停留时间
    }
}



//提示的类型
public enum PopTipType
{
    None,                       //无
    PureText,                   //纯文字
    Level,                      //关卡
}
