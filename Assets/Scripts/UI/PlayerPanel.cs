using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField]
    Button[] buttonList = null;
    string[] buttonTypes = new string[]
    {
        "head","body","leg","shoes","belt","shoulder","leftWeapon","rightWeapon",
    };

    public void Init(Inventory inventory)
    {
        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].gameObject.name = buttonTypes[i];
            buttonList[i].GetComponentInChildren<Text>().text = buttonTypes[i];
        }
    }
}
