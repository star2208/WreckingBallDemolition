using RayFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public List<BuildingFrag> fragList = new List<BuildingFrag>();

    public float maxDistance;

    public int damageNum;
    public AnimationCurve damageCurve;

    private SphereCollider coll;

    private void Awake()
    {
        coll = GetComponent<SphereCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        coll.radius = maxDistance = 8 + 7f * 1f / 20f * GameDataManager.instance.sizeLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool isBomb = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fragment"))
        {
            BuildingFrag frag = other.GetComponent<BuildingFrag>();
            fragList.Add(frag);
            TakeDamage();
            //isBomb = true;
            //GetComponent<Collider>().enabled = false;
            Destroy(gameObject, 0.2f);
        }
    }

    private bool isBreak = false;
    private bool isTakeDamage = false;

    private void TakeDamage()
    {
        for (int i = 0; i < fragList.Count; i++)
        {
            float pre = (fragList[i].transform.position - transform.position).magnitude / maxDistance;
            pre = Mathf.Clamp(pre, 0f, 1f);
            fragList[i].HP -= (int)(damageCurve.Evaluate(pre) * damageNum);
            if (fragList[i].HP <= 0)
                isBreak = true;
            //Debug.Log(fragList[i].name + " : " + fragList[i].HP + " | " + (int)damageCurve.Evaluate(pre) * damageNum);
        }
        if (!isBomb)
        {
            RayfireBomb bomb = Instantiate(GameDataManager.instance.bombPre, transform.position, Quaternion.identity).GetComponent<RayfireBomb>();
            //设置爆炸的范围和威力
            //bomb.GetComponent<RayfireBomb>().range = ;
            bomb.GetComponent<RayfireBomb>().strength = 0.5f + 9.5f * 1f / 20f * GameDataManager.instance.sizeLevel; 
            bomb.Explode(0.15f);
            Destroy(bomb.gameObject, 1.0f);
            isBomb = true;
        }
        if (isBreak && !isTakeDamage)
        {
            UIGamePage.instance.AddMoney(transform.position);
            isTakeDamage = true;
        }
    }

    public int GetDamage(Vector3 pos)
    {
        float pre = (pos - transform.position).magnitude / maxDistance;
        pre = Mathf.Clamp(pre, 0f, 1f);
        return (int)(int)(damageCurve.Evaluate(pre) * damageNum);
    }
}
