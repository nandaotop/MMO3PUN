using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    float rotSpeed = 2;

    public void Tick(Transform follow)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, follow.rotation, rotSpeed * Time.deltaTime);
    }
}
