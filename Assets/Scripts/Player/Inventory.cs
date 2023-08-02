using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<Pair<Skill, int>> equippedSKills = new List<Pair<Skill, int>>();
    public List<Skill> skills = new List<Skill>();
    public List<Item> items = new List<Item>();
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
        var allSkills = Resources.LoadAll<Skill>("");
        foreach (var d in data.equip)
        {
            var item = GetItem<Item>(allItems, d.Key);
            if (item != null)
            {
                SetEquip(item as Equip, d.value);
            }
        }
        foreach (var d in data.items)
        {
            var item = GetItem<Item>(allItems, d);
            if (item != null)
            {
                items.Add(item);
            }
        }
        foreach (var d in data.skills)
        {
            var skill = GetItem<Skill>(allSkills, d);
            if (skill != null)
            {
                skills.Add(skill);
            }
        }
        foreach (var d in data.equipSkills)
        {
            var skill = GetItem<Skill>(allSkills, d.Key);
            if (skill != null)
            {
                var pair = new Pair<Skill, int>() {Key = skill, value = d.value};
                equippedSKills.Add(pair);
            }
        }
    }

    public void SetEquip(Equip equip, int id = 0)
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

        if (lastEquip != null)
        {
            items.Add(lastEquip);
            lastEquip = null;
        }
    }

    public void RemoveItem(Equip e)
    {
        foreach (var i in items)
        {
            if (i.name == e.name)
            {
                items.Remove(i);
                break;
            }
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

    public void UpdateSKill(ActionController controller)
    {
        equippedSKills.Clear();
        for (int i = 0; i < controller.actions.Count; i++)
        {
            var skill = controller.actions[i].skill;
            if (skills != null)
            {
                var pair = new Pair<Skill, int>() {Key = skill, value = i};
                equippedSKills.Add(pair);
            }
        }
    }
}

[System.Serializable]
public class Pair<T1, T2>
{
    public T1 Key;
    public T2 value;
}
