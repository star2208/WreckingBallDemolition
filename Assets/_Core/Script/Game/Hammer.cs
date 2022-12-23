using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public float distance;

    public float ballRoateSpeed;
    public Transform ballTrans;
    public Transform chainTrans;
    public GameObject[] outward;

    private Rigidbody m_rigid;
    private HingeJoint joint;

    private GameDataManager data;

    private void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        joint = GetComponent<HingeJoint>();
    }

    // Start is called before the first frame update
    void Start()
    {
        data = GameDataManager.instance;

        transform.localScale = Vector3.one * (0.4f + 0.6f * 1 / 20 * data.sizeLevel);

        if (data.powerLevel > 0)
            outward[Mathf.Clamp(data.powerLevel - 1, 0, outward.Length - 1)].SetActive(true);
        //joint.connectedAnchor = transform.position;
        //transform.eulerAngles = transform.position.x >= 0 ? Vector3.forward * 80 : Vector3.back * 80;
        Destroy(gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        ballTrans.localEulerAngles += Vector3.up * ballRoateSpeed * Time.deltaTime;

        if (!isHit)
            m_rigid.AddForce(Physics.gravity * m_rigid.mass * 8);
    }

    private bool isHit = false;
    private bool hitOther = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isHit)
            return;


        if (collision.gameObject.layer == LayerMask.NameToLayer("Fragment"))
        {
            GameObject hitbox = Instantiate(data.hitBoxPre, collision.contacts[0].point, Quaternion.identity);
            Destroy(gameObject, 1f);
            GameObject hitEffect = Instantiate(data.hitEffectPre, collision.contacts[0].point, Quaternion.identity);
            Destroy(hitEffect, 1.5f);
            CameraManager.instance.CameraShake();
            isHit = true;
            m_rigid.useGravity = true;
            AudioSource hit;
            if (data.hitList.Count < 3)
            {
                hit = Instantiate(data.audio_HitPre, data.transform).GetComponent<AudioSource>();
                data.hitList.Add(hit);
            }
            else
            {
                hit = data.hitList[data.hitIndex % 2];
                data.hitIndex++;
            }
            hit.clip = data.hitClips[Random.Range(0, data.hitClips.Length)];
            hit.volume = 0.3f + 0.6f * data.sizeLevel / 20;
            hit.Play();
            Destroy(transform.parent.gameObject, 1f);
            return;
        }

        if (hitOther)
            return;

        hitOther = true;
        Destroy(transform.parent.gameObject, 1f);
    }

    public void SetJoint()
    {
        transform.localPosition = Vector3.zero;
        joint.anchor = Vector3.up * distance;
        transform.localEulerAngles = new Vector3(0, 0, 90);
    }
}
