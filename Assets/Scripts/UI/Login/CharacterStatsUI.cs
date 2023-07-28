using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsUI : MonoBehaviour
{
    [SerializeField]
    Text Name = null, Hp = null, Lv = null;
    [SerializeField]
    Text Atk = null, Def = null, Mana = null;
    public void SetUp(SaveData data)
    {
        
    }
}
