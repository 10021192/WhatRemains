using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemManaPotion", menuName = "Items/Mana Potion")]
public class ItemManaPotion : InventoryItem
{
    [Header("Config")]
    public int ManaValue;

    private static float lastUseTime = -15f; // Ensure potions are usable immediately when the game starts.
    private const float CooldownDuration = 15f; // Cooldown in seconds.

    public override bool UseItem()
    {
        if (Time.time - lastUseTime < CooldownDuration)
        {
            return false;
        }

        if (GameManager.Instance.Player.PlayerMana.CanRecoverMana())
        {
            GameManager.Instance.Player.PlayerMana.RecoverMana(ManaValue);
            lastUseTime = Time.time; // Update the last use time.
            return true;
        }

        return false;
    }
}