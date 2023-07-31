using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsUI : MonoBehaviour
{
    [SerializeField]
    Text Name = null, Hp = null, Lv = null;
    [SerializeField]
    Text Atk = null, Def = null, Mana = null, Class = null;
    [SerializeField]
    GameObject[] slots = null;

    public void SetUp(SaveData data)
    {
        Stats stat = data.stat;
        
        Name.text = "Name: " + data.characterName;
        Hp.text = "Hp: " + stat.HP();
        Lv.text = "Lv: " + stat.Level;
        Atk.text = "Atk: " + stat.PhysicalAttack;
        Def.text = "Def: " + stat.PhysicalDefense;
        Mana.text = "Mana: " + stat.Mana();
        Class.text = "Class: " + stat.charClass.ToString();

        foreach (var i in slots)
        {
            i.transform.GetChild(0).GetComponent<Image>().enabled = false;
        }

        if (data.equip.Count > 0)
        {
            for (int i = 0; i < data.equip.Count; i++)
            {
                Equip e = Resources.Load<Equip>("Items/" + data.equip[i]);
                if (e != null)
                    SetEquip(e.sprite, i);
            }
        }
    }

    void SetEquip(Sprite s, int index)
    {
        var img = slots[index].transform.GetChild(0).GetComponent<Image>();
        img.sprite = s;
        img.enabled = true;
    }
}
