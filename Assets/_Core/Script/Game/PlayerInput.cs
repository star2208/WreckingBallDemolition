using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerInput : MonoBehaviour
{
    public static PlayerInput instance;

    public bool canOperate = false;

    private GameManager gm;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canOperate)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            CreateHammer(Input.mousePosition, true);
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit hit;
            //if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Fragment")))
            //{
            //    Vector3 offset = hit.point - gm.curBuilding.hammerConnectTrans.position;

            //    float an = Vector3.Angle(Vector3.right, new Vector3(offset.x, 0, offset.z));
            //    if (offset.z > 0)
            //        an = -an;

            //    if (!GameManager.instance.curBuilding.allowBothSide)
            //    {
            //        if (an < 0)
            //            an = an + 180f;
            //    }
            //    //if(an > 180)
            //        //an = 360 - an;
            //    GameObject ball = Instantiate(GameDataManager.instance.hammerPre, gm.curBuilding.hammerConnectTrans);
            //    ball.transform.localPosition = Vector3.zero;
            //    ball.transform.eulerAngles = Vector3.up * an;
            //    Hammer hammer = ball.GetComponentInChildren<Hammer>();
            //    //hammer.distance = Mathf.Abs(offset.y);
            //    hammer.distance = (offset).magnitude;
            //    hammer.SetJoint();
            //    UIGamePage.instance.CreateIcon(hit.point);
        }

    }

    public void CreateHammer(Vector3 pos, bool isHand = false)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Fragment")))
        {
            if (isHand)
                UIGamePage.instance.ShowHand(Camera.main.WorldToScreenPoint(hit.point), true);
            Vector3 offset = hit.point - gm.curBuilding.hammerConnectTrans.position;

            float an = Vector3.Angle(Vector3.right, new Vector3(offset.x, 0, offset.z));
            if (offset.z > 0)
                an = -an;

            if (!GameManager.instance.curBuilding.allowBothSide)
            {
                if (an < 0)
                    an = an + 180f;
            }
            //if(an > 180)
            //an = 360 - an;
            GameObject ball = Instantiate(GameDataManager.instance.hammerPre, gm.curBuilding.hammerConnectTrans);
            ball.transform.localPosition = Vector3.zero;
            ball.transform.eulerAngles = Vector3.up * an;
            Hammer hammer = ball.GetComponentInChildren<Hammer>();
            //hammer.distance = Mathf.Abs(offset.y);
            hammer.distance = (offset).magnitude;
            hammer.SetJoint();
            UIGamePage.instance.CreateIcon(hit.point);
        }
    }
}

