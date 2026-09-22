using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Magic,
    Melee
}

[CreateAssetMenu(fileName = "Weapon_")]
public class Weapon : ScriptableObject
{
    [Header("Config")]
    public Sprite Icon;
    public WeaponType WeaponType;
    public int Damage;

    [Header("Projectile")]
    public Projectile ProjectilePrefab;
    public int RequiredMana;
}
