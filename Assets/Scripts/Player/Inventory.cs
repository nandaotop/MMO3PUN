using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<Skill> playerSKills = new List<Skill>();
    public List<Skill> equippedSKills = new List<Skill>();
    public Equip head, body, leg, shoes, belt, shoulder;
    public Equip leftWeapon, rightWeapon;
    public Equip lastEquip;

    public List<Equip> AllEquip()
    {
        List<Equip> equipList = new List<Equip>();
        equipList.Add(head);
        equipList.Add(body);
        equipList.Add(leg);
        equipList.Add(shoes);
        equipList.Add(belt);
        equipList.Add(shoulder);
        equipList.Add(leftWeapon);
        equipList.Add(rightWeapon);
        return equipList;
    }

    public void Init(SaveData data)
    {
        var allItems = Resources.LoadAll<Item>("");
        foreach (var d in data.equip)
        {
            var item = GetItem<Item>(allItems, d);
            if (item != null)
            {
                SetEquip(item as Equip);
            }
        }
    }

    public void SetEquip(Equip equip)
    {
        switch (equip.type)
        {
            case EquipType.head:
                lastEquip = head;
                head = equip;
                break;
            case EquipType.body:
                lastEquip = body;
                body = equip;
                break;
            case EquipType.leg:
                lastEquip = leg;
                leg = equip;
                break;
            case EquipType.shoes:
                lastEquip = shoes;
                shoes = equip;
                break;
            case EquipType.belt:
                lastEquip = belt;
                belt = equip;
                break;
            case EquipType.shoulder:
                lastEquip = shoulder;
                shoulder = equip;
                break;
            case EquipType.leftWeapon:
                lastEquip = leftWeapon;
                leftWeapon = equip;
                break;
            case EquipType.rightWeapon:
                lastEquip = rightWeapon;
                rightWeapon = equip;
                break;
        }
    }

    T GetItem<T>(T[] arr, string s) where T: ScriptableObject
    {
        foreach (var a in arr)
        {
            if (a.name == s)
            {
                return a;
            }
        }
        return null;
    }
}
