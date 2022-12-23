using DG.Tweening;
using RayFire;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFrag : MonoBehaviour
{
    private Building building;
    private Rigidbody m_rigid;

    private GameDataManager data;

    public int HP
    {
        get { return hp; }
        set 
        {
            if (hp <= 0)
                return;

            if (value < hp)
            { 
            
            }

            hp = value;

            if (hp <= 0)
            {
                FragBreak();
                isBroken = true;
            }
        }
    }
    private int hp = 100;
    private int maxHP = 100;

    private void FragBreak()
    {
        RayfireRigid rf = GetComponent<RayfireRigid>();//.Activate();

        if (rf)
        {
            rf.Activate();
        }

        Rigidbody rigid = GetComponent<Rigidbody>();
        if (rigid)
        {
            rigid.isKinematic = false;
            rigid.velocity = Random.onUnitSphere * Random.Range(1f, 5f);
        }
        else
            return;

        BrokenFrag();
        //gameObject.layer = LayerMask.NameToLayer("Debris");
        //Invoke("Disappear", 10f);
        //Destroy(this);
        //Destroy(GetComponent<RayfireRigid>());
    }

    private bool isBroken = false;

    private void Awake()
    {
        building = transform.GetComponentInParent<Building>();
        if (building)
        {
            maxHP = building.fragHP;
            hp = maxHP;
        }
    }

    private void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
        data = GameDataManager.instance;
    }

    private void Update()
    {
        if (isBroken)
            return;

        if (m_rigid)
            return;

        m_rigid = GetComponent<Rigidbody>();
        if (m_rigid)
            BrokenFrag();
    }

    private void BrokenFrag()
    {
        transform.SetParent(building.fragArea);
        gameObject.layer = LayerMask.NameToLayer("Debris");
        if (building.fragRemainList.Contains(gameObject))
            building.fragRemainList.Remove(gameObject);
        AudioSource hit = null;
        if (data.breakList.Count < 5)
        {
            hit = Instantiate(data.audio_HitPre, data.transform).GetComponent<AudioSource>();
            data.breakList.Add(hit);
        }
        else
        {
            //if (data.canTakeBreakAudio)
            //{
            //    hit = data.breakList[data.breakIndex % 2];
            //    data.breakIndex++;
            //    data.takeBreakAudio = true;
            //}
            for (int i = 0; i < data.breakList.Count; i++)
            {
                if (!data.breakList[i].isPlaying || data.breakList[i].time >= 1.5f)
                {
                    hit = data.breakList[i];
                    break;
                }
            }
        }
        if (hit)
        {
            hit.clip = data.breakClips[Random.Range(0, data.breakClips.Length)];
            hit.volume = 0.1f;
            hit.Play();
        }
        Invoke("Disappear", 10f);
        isBroken = true;
    }

    private void Disappear()
    {
        transform.DOScale(Vector3.zero, 0.8f).OnComplete(()=> Destroy(gameObject));
    }
}
