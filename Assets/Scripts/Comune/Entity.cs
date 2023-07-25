using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Entity : MonoBehaviourPun
{
    public AnimatorSync sync;
    public Rigidbody rb;
    public float moveSpeed = 3;
    public float moveMultipler = 1;
    public bool isDeath;
    [SerializeField]
    int hp = 10; // TODO: utilizar float ao fim do curso

    PhotonView view;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (photonView.IsMine == false) return;
        Tick();
    }

    public virtual void Init()
    {
        sync = GetComponentInChildren<AnimatorSync>();
        sync.Init();
        rb = GetComponent<Rigidbody>();
        view = PhotonView.Get(this);
    }
    
    public virtual void Tick()
    {

    }

    public void TakeDamage(int dmg)
    {
        if (PhotonNetwork.IsConnected)
        {
            DebugDamage(dmg);
        }
        else
        {
            if (view == null) view = PhotonView.Get(this);
            view.RPC("DealDamage", RpcTarget.MasterClient, dmg);
        }

        
    }
    
    public void DebugDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log(hp);
        if (hp <= 0)
        {
            hp = 0;
            isDeath = true;
            sync.IsDead(true);
        }
    }

    [PunRPC]
    public void DealDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log(hp);
        if (hp <= 0)
        {
            hp = 0;
            isDeath = true;
            sync.IsDead(true);
        }

        view.RPC("SyncronizeStat", RpcTarget.All, hp);
    }

    [PunRPC]
    public void SyncronizeStat(int hp)
    {
        this.hp = hp;
    }
}
