using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ViewCamera : MonoBehaviour
{
    public static ViewCamera instance;

    public Transform viewBallTrans;

    public GameObject[] ballPowerLevel;

    private GameDataManager data;

    private float sceneWidth;
    private float sceneHeight;
    private float sceneY;
    private Camera viewCam;

    private void Awake()
    {
        instance = this;
        viewCam = GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        data = GameDataManager.instance;
        viewBallTrans.localScale = Vector3.one * 0.4f;
        sceneWidth = viewCam.rect.width;
        sceneHeight = viewCam.rect.height;
        sceneY = viewCam.rect.y;
    }

    private bool isAdd = true;

    // Update is called once per frame
    void Update()
    {
        viewBallTrans.eulerAngles += Vector3.up * 180 * Time.deltaTime;

        if (isAddSize)
        {
            if (isAdd)
            {
                rate += Time.deltaTime;
                if (rate >= 1.1f)
                    isAdd = false;
            }
            else
            {
                rate -= Time.deltaTime;
                if (rate <= 1.0f)
                {
                    isAdd = true;
                    isAddSize = false;
                }
            }
            rate = Mathf.Clamp(rate, 1.0f, 1.1f);
            viewCam.rect = new Rect(0, sceneY - ((sceneHeight * rate - sceneHeight) / 2), sceneWidth * rate, sceneHeight * rate);
        }
    }

    private bool isAddSize = false;
    private float rate = 1.0f;
    
    public void AddSize()
    {
        viewBallTrans.DOPunchScale(Vector3.one * 0.2f, 0.2f, 0, 0)
            .OnComplete(() => viewBallTrans.localScale = Vector3.one * (0.4f + 0.6f * data.sizeLevel / 20));
        //viewBallTrans.localScale = Vector3.one * (0.4f + 0.6f * data.sizeLevel / 20);
        isAddSize = true;
        isAdd = true;
    }

    public void AddPower()
    {
        for (int i = 0; i < ballPowerLevel.Length; i++)
        {
            ballPowerLevel[i].SetActive(i == data.powerLevel - 1);
        }
        viewBallTrans.DOPunchScale(Vector3.one * 0.2f, 0.2f, 0, 0)
            .OnComplete(() => viewBallTrans.localScale = Vector3.one * (0.4f + 0.6f * data.sizeLevel / 20));
    }
}
