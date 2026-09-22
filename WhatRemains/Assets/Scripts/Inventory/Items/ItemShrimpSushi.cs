using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemShrimpSushi", menuName = "Items/Shrimp Sushi")]
public class ItemShrimpSushi : InventoryItem
{
    [Header("Config")]
    public int RegenAmount;
    public int RegenInterval;
    public int RegenDuration;

    public override bool UseItem()
    {
        if (!GameManager.Instance.Player.PlayerMana.IsRegenActive)
        {
            GameManager.Instance.Player.PlayerMana.StartManaRegen(RegenAmount, RegenInterval, RegenDuration);
            return true;
        }
        Debug.Log("You must wait until the current regeneration effect ends before using another item.");
        return false;
    }
}
