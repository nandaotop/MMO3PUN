using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats 
{
    public int Level = 1;
    public int HP = 1;
    int ManaMultiplier = 2;
    public int Mana = 1;
    public int ManaXsecond()
    {
        float regen = Mana / 10f;
        return (int)regen;
    }
    public float PhysicalAttack = 1;
    public float Wisdom = 1;
    public float MagicalAttack = 1;
    public float PhysicalDefense = 1;
    public float MagicalDefense = 1;
    public float Agility = 1;
    public float Aggro = 1;
    public CharacterClass charClass = CharacterClass.warrior;


    // deletar
    public int Stamina = 1;
    public int Strenght = 1;
    public int Intellect = 1;
    public int AgilityInt = 1;
    public int Armor = 1;
}

public enum CharacterClass
{
    warrior,
    knight,
    mercenary,
    magician,
    priest,
    assassin,
    hunter,
    warlock,
    sage,
}

