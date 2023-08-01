using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField]
    Button[] buttonList = null;
    [SerializeField]
    GameObject slot = null;
    List<GameObject> currentSlots = new List<GameObject>();
    Inventory inventory;
    string[] buttonTypes = new string[]
    {
        "head","body","leg","shoes","belt","shoulder","leftWeapon","rightWeapon",
    };

    public void Init(Inventory inventory)
    {
        this.inventory = inventory;

        for (int i = 0; i < buttonList.Length; i++)
        {
            buttonList[i].gameObject.name = buttonTypes[i];
            buttonList[i].GetComponentInChildren<Text>().text = buttonTypes[i];
            int id = i;
            buttonList[i].onClick.AddListener(delegate
            {
                OnPressButton(id);
            });
        }
    }

    public void OnPressButton(int id)
    {
        EquipType t = EquipType.head;
        switch (id)
        {
            case 1:
                t = EquipType.body;
                break;
            case 2:
                t = EquipType.leg;
                break;
            case 3:
                t = EquipType.shoes;
                break;
            case 4:
                t = EquipType.belt;
                break;
            case 5:
                t = EquipType.shoulder;
                break;
            case 6:
                t = EquipType.leftWeapon;
                break;
            case 7:
                t = EquipType.rightWeapon;
                break;
        }
        ShowSlots(t);
    }

    void ShowSlots(EquipType t)
    {
        foreach (var obj in currentSlots)
        {
            Destroy(obj);
        }
        currentSlots.Clear();
        var arr = inventory.items;
    }
}
