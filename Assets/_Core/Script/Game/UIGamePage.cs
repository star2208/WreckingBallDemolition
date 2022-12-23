using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGamePage : MonoBehaviour
{
    public static UIGamePage instance;

    public Button btn_Size;
    public Button btn_Power;
    public Button btn_Income;
    public GameObject dollar_Size;
    public GameObject dollar_Power;
    public GameObject dollar_Income;
    public GameObject max_Size;
    public GameObject max_Power;
    public GameObject max_Income;
    public GameObject mask_Size;
    public GameObject mask_Power;
    public GameObject mask_Income;
    private UIButton uib_Size;
    private UIButton uib_Power;
    private UIButton uib_Income;

    public TextMeshProUGUI txt_Money;
    public TextMeshProUGUI txt_SizeCost;
    public TextMeshProUGUI txt_PowerCost;
    public TextMeshProUGUI txt_IncomeCost;
    public TextMeshProUGUI txt_SizeAdd;
    public TextMeshProUGUI txt_PowerAdd;
    public TextMeshProUGUI txt_IncomeAdd;
    public TextMeshProUGUI txt_BuildingValue;
    public TextMeshProUGUI txt_BuildingProcess;

    public Slider slider_Progress;

    [Space]
    public Transform moneyTrans;
    public Transform hand;

    private GameDataManager data;
    private UIDamageNumArea hitMoney;

    private void Awake()
    {
        instance = this;
        hitMoney = transform.GetComponentInChildren<UIDamageNumArea>();
        uib_Size = btn_Size.GetComponent<UIButton>();
        uib_Power = btn_Power.GetComponent<UIButton>();
        uib_Income = btn_Income.GetComponent<UIButton>();
    }

    // Start is called before the first frame update
    void Start()
    {
        data = GameDataManager.instance;
        //txt_BuildingProcess.color = Color.white;
        SetText();
        max_Size.SetActive(false);
        max_Power.SetActive(false);
        max_Income.SetActive(false);
        mask_Size.SetActive(false);
        mask_Power.SetActive(false);
        mask_Income.SetActive(false);
        dollar_Size.SetActive(false);
        dollar_Power.SetActive(false);
        dollar_Income.SetActive(false);
        slider_Progress.gameObject.SetActive(false);
        hand.gameObject.SetActive(false);

        uib_Size.PointerDown.AddListener(() =>
        {
            uib_Size.GetComponentInParent<TweenScale>().In();
            ShowHand(btn_Size.transform.position);
        });
        uib_Size.PointerUp.AddListener(() =>
        {
            uib_Size.GetComponentInParent<TweenScale>().Out();
            //hand.gameObject.SetActive(false);
            //hand.localScale = Vector3.one;
        });
        btn_Size.onClick.AddListener(() =>
        {
            data.totalMoney -= data.sizeCost;
            data.sizeLevel++;
            data.sizeAdd = data.sizeLevel * 10;
            data.sizeCost = 190 + data.sizeLevel * 50;
            SetText();
            ViewCamera.instance.AddSize();
            if (data.sizeLevel >= 20)
                max_Size.SetActive(true);
            //ShowHand(btn_Size.transform.position);
            data.audio_Button.Play();
            GameManager.instance.curBuilding.GetPosition();
        });
        uib_Power.PointerDown.AddListener(() =>
        {
            uib_Power.GetComponentInParent<TweenScale>().In();
            ShowHand(btn_Power.transform.position);
        });
        uib_Power.PointerUp.AddListener(() =>
        {
            uib_Power.GetComponentInParent<TweenScale>().Out();
            //hand.gameObject.SetActive(false);
            //hand.localScale = Vector3.one;
        });
        btn_Power.onClick.AddListener(() =>
        {
            data.totalMoney -= data.powerCost;
            data.powerLevel++;
            data.powerAdd = data.powerLevel * 10;
            data.powerCost = 190 + data.powerLevel * 50;
            SetText();
            ViewCamera.instance.AddPower();
            if (data.powerLevel >= 18)
                max_Power.SetActive(true);
            //ShowHand(btn_Power.transform.position);
            data.audio_Button.Play();
            GameManager.instance.curBuilding.GetPosition();
        });
        uib_Income.PointerDown.AddListener(() =>
        {
            uib_Income.GetComponentInParent<TweenScale>().In();
            ShowHand(btn_Income.transform.position);
        });
        uib_Income.PointerUp.AddListener(() =>
        {
            uib_Income.GetComponentInParent<TweenScale>().Out();
            //hand.gameObject.SetActive(false);
            //hand.localScale = Vector3.one;
        });
        btn_Income.onClick.AddListener(() =>
        {
            data.totalMoney -= data.incomeCost;
            data.incomeLevel++;
            data.incomeAdd = data.incomeLevel * 10;
            data.incomeCost = 190 + data.incomeLevel * 50;
            data.hitIncome = 20 + data.incomeLevel * 10;
            SetText();
            if (data.incomeLevel >= 12)
                max_Income.SetActive(true);
            //ShowHand(btn_Income.transform.position);
            data.audio_Button.Play();
        });
    }

    // Update is called once per frame
    void Update()
    {
        BtnEnable();
        slider_Progress.value = GameManager.instance.curBuilding.destroyPer;
    }

    public void CreateIcon(Vector3 pos)
    {
        GameObject go = Instantiate(data.uiWarningPre, transform);
        go.transform.position = Camera.main.WorldToScreenPoint(pos);
        go.transform.DOPunchScale(Vector3.one * 0.5f, 0.5f, 0, 0).SetLoops(-1);
        Destroy(go, 1.5f);
    }

    private void SetText()
    {
        txt_Money.text = data.totalMoney.ToString();
        txt_SizeAdd.text = "+" + data.sizeAdd.ToString() + "%";
        txt_PowerAdd.text = "+" + data.powerAdd.ToString() + "%";
        txt_IncomeAdd.text = "+" + data.incomeAdd.ToString();
        txt_SizeCost.text = data.sizeCost.ToString();
        txt_PowerCost.text = data.powerCost.ToString();
        txt_IncomeCost.text = data.incomeCost.ToString();
    }

    private void BtnEnable()
    {
        btn_Size.interactable = uib_Size.enabled = (data.totalMoney >= data.sizeCost && data.sizeLevel < 20);
        dollar_Size.SetActive(!btn_Size.interactable);
        mask_Size.SetActive(data.totalMoney < data.sizeCost || data.sizeLevel >= 20);
        btn_Power.interactable = uib_Power.enabled = (data.totalMoney >= data.powerCost && data.powerLevel < 18);
        dollar_Power.SetActive(!btn_Power.interactable);
        mask_Power.SetActive(data.totalMoney < data.powerCost || data.powerLevel >= 18);
        btn_Income.interactable = uib_Income.enabled = (data.totalMoney >= data.incomeCost && data.incomeLevel < 12);
        dollar_Income.SetActive(!btn_Income.interactable);
        mask_Income.SetActive(data.totalMoney < data.incomeCost || data.incomeLevel >= 12);
    }

    public void AddMoney(Vector3 pos)
    {
        Vector3 uiPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.GetComponentInParent<Canvas>().GetComponent<RectTransform>(),
            Camera.main.WorldToScreenPoint(pos), Camera.main, out uiPos);
        //Vector3 uiPos = Camera.main.WorldToScreenPoint(pos);
        hitMoney.Add(data.hitIncome, uiPos, false);

        StartCoroutine(CreateDollar(Camera.main.WorldToScreenPoint(pos), data.hitIncome / 10));
    }

    IEnumerator AllBuildingMoney(Vector3 pos, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Transform trans = Instantiate(data.dollarLPre, moneyTrans).transform;
            trans.position = pos;
            trans.DOLocalMove(Vector3.zero, 1.2f).OnComplete(() =>
            {
                data.totalMoney += 500;
                txt_Money.text = data.totalMoney.ToString();
                Destroy(trans.gameObject);
            });
            yield return new WaitForSeconds(data.hitIncome >= 100 ? (0.8f / count) : 0.08f);
        }
    }

    IEnumerator CreateDollar(Vector3 pos, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Transform trans = Instantiate(data.dollarPre, moneyTrans).transform;
            trans.position = pos;
            trans.DOLocalMove(Vector3.zero, 1.2f).OnComplete(() =>
            {
                data.audio_Money.Play();
                data.totalMoney += 10;
                txt_Money.text = data.totalMoney.ToString();
                Destroy(trans.gameObject);
            });
            yield return new WaitForSeconds(data.hitIncome >= 100 ? (0.8f / count) : 0.08f);
        }
    }

    public void ShowBuildingVlue()
    {
        txt_BuildingValue.transform.position = Camera.main.WorldToScreenPoint(GameManager.instance.curBuilding.topTrans.position);
        txt_BuildingValue.text = "$" + GameManager.instance.curBuilding.buildingValue.ToString();
        //txt_BuildingValue.gameObject.SetActive(true);
        txt_BuildingValue.GetComponent<TweenAlpha>().In();
        //txt_BuildingProcess.color = Color.white;
        txt_BuildingProcess.text = "$" + GameManager.instance.curBuilding.buildingValue;
        slider_Progress.value = 0;
        slider_Progress.gameObject.SetActive(true);
        Invoke("HideBuildingValue", 1.2f);
    }

    private void HideBuildingValue()
    {
        txt_BuildingValue.GetComponent<TweenAlpha>().Out();
    }

    public void BuildingComplete()
    {
        txt_BuildingValue.text = "+" + GameManager.instance.curBuilding.buildingValue.ToString();
        txt_BuildingValue.GetComponent<TweenAlpha>().In();
        data.totalMoney += GameManager.instance.curBuilding.buildingValue;
        txt_Money.text = data.totalMoney.ToString();
        //txt_BuildingProcess.color = Color.green;
        txt_BuildingProcess.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 0, 1);
        txt_BuildingValue.transform.DOMove(txt_BuildingValue.transform.position + Vector3.up * 100, 0.8f)
            .OnComplete(() =>
            {
                txt_BuildingValue.GetComponent<TweenAlpha>().Out();
                slider_Progress.gameObject.SetActive(false);
            });
        data.audio_MoneyL.Play();
        StartCoroutine(AllBuildingMoney(Camera.main.WorldToScreenPoint(GameManager.instance.curBuilding.transform.position),
            GameManager.instance.curBuilding.buildingValue / 500));
    }

    private Tweener tween_Hand = null;

    public void ShowHand(Vector3 pos, bool isHand = false)
    {
        if (tween_Hand != null && tween_Hand.IsActive())
        {
            hand.localScale = Vector3.one;
            tween_Hand.Kill();
            tween_Hand = null;
        }
        hand.localScale = Vector3.one;
        if (isHand)
            hand.position = pos;
        else
            hand.position = pos + Vector3.up * 20;
        
        hand.gameObject.SetActive(true);
        tween_Hand = hand.DOScale(Vector3.one * 0.6f, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            hand.gameObject.SetActive(false);
            hand.localScale = Vector3.one;
        });
    }
}
