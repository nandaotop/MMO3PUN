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

    public override void Init()
    {
        base.Init();
        if (!photonView.IsMine) return;
        controller = GetComponent<ActionController>();
        var f = Resources.Load<CameraFollow>(StaticStrings.follow);
        follow = Instantiate(f, transform.position, transform.rotation);
        follow.Init(transform);
    }

    public override void Tick()
    {
        UseCamera();
        if (!CanMove()) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector3 move = (transform.right * x) + (transform.forward * y);
        move *= Time.deltaTime * moveMultipler * moveSpeed;
        move.y = rb.velocity.y;
        rb.velocity = move;
        sync.Move(x, y);
        controller.Tick(follow.transform);
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
}
