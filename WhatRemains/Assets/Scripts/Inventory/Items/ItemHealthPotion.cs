using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemHealthPotion", menuName = "Items/Health Potion")]
public class ItemHealthPotion : InventoryItem
{
    [Header("Config")]
    public int HealthValue;

    private static float lastUseTime = -15f; // Ensure potions are usable immediately when the game starts.
    private const float CooldownDuration = 15f; // Cooldown in seconds.

    public override bool UseItem()
    {
        if (Time.time - lastUseTime < CooldownDuration)
        {
            return false;
        }

        if(GameManager.Instance.Player.PlayerHealth.CanRestoreHealth())
        {
            GameManager.Instance.Player.PlayerHealth.RestoreHealth(HealthValue);
            lastUseTime = Time.time; // Update the last use time.
            return true;
        }
        return false;
    }
}
