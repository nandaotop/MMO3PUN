using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats 
{
    public int Level = 1;
    public int HP = 1; // TODO: mudar para float
    public float PhysicalAttack = 1;
    public float MagicalAttack = 1;
    public float PhysicalDefense = 1;
    public float MagicalDefense = 1;
    public float Agility = 1;
    public float Aggro = 1;
    public CharacterClass charClass = CharacterClass.warrior;
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

