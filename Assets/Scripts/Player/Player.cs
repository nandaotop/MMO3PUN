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

    public override void Init()
    {
        base.Init();
        if (!photonView.IsMine) return;

        data = CharacterCreate.selectedData;

        data = SaveManager.LoadData<SaveData>(data.characterName);
        if (data == null)
        {
            data = new SaveData();
        }

        controller = GetComponent<ActionController>();
        controller.sync = sync;
        controller.Init(this);
        var f = Resources.Load<CameraFollow>(StaticStrings.follow);
        follow = Instantiate(f, transform.position, transform.rotation);
        follow.Init(transform);
        WorldManager.instance.playerList.Add(transform);
        UIManager.instance.player = this;
        OnDeathEvent = () => 
        {
            UIManager.instance.deathPanel.SetActive(true);
        };
    }

    public override void Tick()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveManager.SaveData(data.characterName, data);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            data = SaveManager.LoadData<SaveData>(data.characterName);
        }

        UseCamera();
        if (controller.mana < stats.Mana())
        {
            manaCounter -= Time.deltaTime;
            if (manaCounter <= 0)
            {
                manaCounter = second;
                controller.mana += stats.ManaXsecond;
                if (controller.mana > stats.Mana()) controller.mana = stats.Mana();
            }
        }

        if (!CanMove()) return;

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
        float x = Input.GetAxisRaw("Mouse X");
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        Vector3 rot = follow.transform.rotation.eulerAngles;
        follow.transform.rotation = Quaternion.Euler(rot.x, rot.y + x * rotSpeed, rot.z);
        if (scroll != 0)
        {
            float val = scrollAmount * scroll;
            val += follow.cam.fieldOfView;
            val = Mathf.Clamp(val, minZoom, maxZoom);
            follow.cam.fieldOfView = val;
        }
    }

    bool CanMove()
    {
        if (isDeath) return false;

        return true;
    }

    public void Respawn()
    {
        transform.position = WorldManager.instance.respawnPoint.position;
        isDeath = false;
        hp = stats.HP();
        sync.IsDead(false);
        if (Photon.Pun.PhotonNetwork.IsConnected)
        {
            view.RPC("SyncronizeStat", Photon.Pun.RpcTarget.All, hp,maxHp);
        }
    }
}
