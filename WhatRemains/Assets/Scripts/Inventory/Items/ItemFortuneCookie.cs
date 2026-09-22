using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemFortuneCookie", menuName = "Items/Fortune Cookie")]
public class ItemFortuneCookie : InventoryItem
{
    [Header("Config")]
    public float BoostDuration;

    public override bool UseItem()
    {
        if (!GameManager.Instance.Player.PlayerUpgrade.isBoostActive)
        {
            int randomIndex = Random.Range(0, System.Enum.GetValues(typeof(AttributeType)).Length);
            AttributeType randomAttribute = (AttributeType)randomIndex;
            GameManager.Instance.Player.PlayerUpgrade.ApplyTemporaryBoost(randomAttribute, BoostDuration);
            return true;
        }
        return false;
    }
}
