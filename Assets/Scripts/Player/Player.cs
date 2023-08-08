using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    CameraFollow follow;
    [SerializeField]
    float rotSpeed = 2;
    [SerializeField]
    float scrollAmount = 3;
    [SerializeField]
    float minZoom = 10, maxZoom = 120;
    ActionController controller;
    const float second = 1;
    float manaCounter = 1;
    public SaveData data = new SaveData();
    // public bool debug = false;
    public bool CanMove = true;
    [SerializeField]
    GameObject uiMan = null;
    [SerializeField]
    CharacterClass debugClass = CharacterClass.warrior;

    public override void Init()
    {
        base.Init();
        if (!photonView.IsMine) return;

        if (uiMan != null)
        {
            Instantiate(uiMan);
        }

        // if (debug)
        // {
        //     data = SaveManager.LoadData<SaveData>("Debug");
        // }
        // else
        // {
        //     data = CharacterCreate.selectedData;
        //     if (data == null)
        //     {
        //         data = new SaveData();
        //     }
        // }

        data = CharacterCreate.selectedData;
        if (data == null)
        {
            data = new SaveData();
            data.stat = CharacterCreate.GetStat(debugClass);
            data.characterName = "Gustav";
        }

        // data = SaveManager.LoadData<SaveData>(data.characterName);

        controller = GetComponent<ActionController>();
        controller.sync = sync;
        controller.Init(this);
        OnChangeItem();
        hp = maxHp;
        var f = Resources.Load<CameraFollow>(StaticStrings.follow);
        follow = Instantiate(f, transform.position, transform.rotation);
        follow.Init(transform);
        WorldManager.instance.playerList.Add(transform);
        UIManager.instance.SetUpPlayer(this);
        OnDeathEvent = () => 
        {
            UIManager.instance.deathPanel.SetActive(true);
        };
    }

    public override void Tick()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     SaveManager.SaveData(data.characterName, data);
        // }

        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     data = SaveManager.LoadData<SaveData>(data.characterName);
        // }

        if (controller.mana < stats.Mana)
        {
            manaCounter -= Time.deltaTime;
            if (manaCounter <= 0)
            {
                manaCounter = second;
                controller.mana += stats.ManaXsecond();
                if (controller.mana > stats.Mana) controller.mana = stats.Mana;
                UIManager.instance.UpdateMana(controller.mana, maxMana);
            }
        }

        controller.MouseLeft();

        if (!CanMove) return;
        UseCamera();
        if (isDeath) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 move = (transform.right * x) + (transform.forward * y);
        move *= Time.deltaTime * moveMultipler * moveSpeed;
        move.y = rb.velocity.y;
        rb.velocity = move;
        sync.Move(x, y);
        controller.Tick(follow.transform, x, y);
    }

    void UseCamera()
    {
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float val = scrollAmount * scroll;
            val += follow.cam.fieldOfView;
            val = Mathf.Clamp(val, minZoom, maxZoom);
            follow.cam.fieldOfView = val;
        }
        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxisRaw("Mouse X");
            Vector3 rot = follow.transform.rotation.eulerAngles;
            follow.transform.rotation = Quaternion.Euler(rot.x, rot.y + x * rotSpeed, rot.z);
        }
        
    }

    public void Respawn()
    {
        transform.position = WorldManager.instance.respawnPoint.position;
        isDeath = false;
        hp = stats.HP;
        sync.IsDead(false);
        if (Photon.Pun.PhotonNetwork.IsConnected)
        {
            view.RPC("SyncronizeStat", Photon.Pun.RpcTarget.All, hp,maxHp);
        }
    }

    public void LockPlayer()
    {
        CanMove = false;
        rb.velocity = Vector3.zero;
        sync.Move(0, 0);
    }

    public void OnChangeItem()
    {
        int stamina = stats.Stamina + controller.inventory.GetParameter(StaticStrings.stamina);
        int intellect = stats.Intellect + controller.inventory.GetParameter(StaticStrings.intellect);
        CalculateStats(stamina, intellect);
    }

    public override void UpdateUI()
    {
        if (photonView.IsMine)
        {
            UIManager.instance.UpdateHP(hp, maxHp);
        }
        else
        {

        }
    }
}
