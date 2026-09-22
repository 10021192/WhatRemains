using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttributeType
{
    Strength,
    Dexterity,
    Intelligence
}

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Config")]
    public int Level;

    [Header("Health")]
    public int Health;
    public int MaxHealth;

    [Header("Mana")]
    public int Mana;
    public int MaxMana;

    [Header("Exp")]
    public float CurrentExp;
    public float NextLevelExp;
    public float InitialNextLevelExp;
    [Range(1f, 100f)]public float ExpMultiplier;

    [Header("Attack")]
    public int BaseDamage;
    public float CriticalChance;
    public int CriticalDamage;

    [Header("Attributes")]
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int AttributePoints;

    [HideInInspector] public float TotalExp;
    [HideInInspector] public int TotalDamage;

    public void ResetPlayer()
    {
        Health = MaxHealth;
        Mana = MaxMana;
        Level = 1;
        CurrentExp = 0;
        NextLevelExp = InitialNextLevelExp;
        TotalExp = 0;
        BaseDamage = 2;
        CriticalChance = 10;
        CriticalDamage = 50;
        Strength = 0;
        Dexterity = 0;
        Intelligence = 0;
        AttributePoints = 0;
    }
}
