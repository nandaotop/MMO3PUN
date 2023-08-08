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
    public Stats stats = new Stats();
    [SerializeField]
    protected int hp = 10; // TODO: utilizar float ao fim do curso
    public int maxHp;
    public int maxMana;

    protected PhotonView view;
    public System.Action OnDeathEvent;
    public int hpMultipler = 2;
    public int manaMultipler = 2;
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
        if(dmg<=0)
        {
            dmg = 1;
        }
        if(!PhotonNetwork.IsConnected)
        {
            DebugDamage(dmg);
        }
        else
        {
            if(view==null) view = PhotonView.Get(this);
            view.RPC("DealDamage", RpcTarget.All, dmg);
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
        UpdateUI();
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
            if (OnDeathEvent != null)
            {
                OnDeathEvent.Invoke();
            }
        }

        view.RPC("SyncronizeStat", RpcTarget.All, hp, maxHp);
    }

    [PunRPC]
    public void SyncronizeStat(int hp, int max)
    {
        this.hp = hp;
        this.maxHp = max;
        UpdateUI();
    }

    public virtual void UpdateUI()
    {
        
    }

    public void CalculateStats(int stamina, int intellect)
    {
        maxHp = stamina * hpMultipler;
        maxMana = intellect * manaMultipler;
    }
}
