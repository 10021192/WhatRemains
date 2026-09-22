using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemFishSushi", menuName = "Items/Fish Sushi")]
public class ItemFishSushi : InventoryItem
{
    [Header("Config")]
    public int RegenAmount;
    public int RegenInterval;
    public int RegenDuration;

    public override bool UseItem()
    {
        if (!GameManager.Instance.Player.PlayerHealth.IsRegenActive)
        {
            GameManager.Instance.Player.PlayerHealth.StartHealthRegen(RegenAmount, RegenInterval, RegenDuration);
            return true;
        }
        Debug.Log("You must wait until the current regeneration effect ends before using another item.");
        return false;
    }
}
