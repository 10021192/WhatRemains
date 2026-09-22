using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon", fileName = "ItemWeapon")]
public class ItemWeapon : InventoryItem
{
    [Header("Config")]
    public Weapon Weapon;

    public override void EquipItem()
    {
        WeaponManager.Instance.EquipWeapon(Weapon);
    }
}
