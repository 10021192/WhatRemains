using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCard : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCost;
    [SerializeField] private TextMeshProUGUI buyAmount;

    private ShopItem item;
    private int quantity;
    private float initialCost;
    private float currentCost;

    private void Update()
    {
        buyAmount.text = quantity.ToString();
        itemCost.text = currentCost.ToString();
    }

    public void ConfigShopCard(ShopItem shopItem)
    {
        item = shopItem;
        itemIcon.sprite = shopItem.Item.Icon;
        itemName.text = shopItem.Item.Name;
        itemCost.text = shopItem.Cost.ToString();
        quantity = 1;
        initialCost = shopItem.Cost;
        currentCost = shopItem.Cost;
    }

    public void BuyItem()
    {
        if(CoinManager.Instance.Coins >= currentCost)
        {
            Inventory.Instance.AddItem(item.Item, quantity);
            CoinManager.Instance.RemoveCoins(currentCost);
            quantity = 1;
            currentCost = initialCost;
            if (item.Item is ItemWeapon weapon && weapon.Weapon.WeaponType == WeaponType.Melee)
            {
                QuestManager.Instance.AddProgress("WeaponCollector", 1);
            }
        }
    }

    public void Add()
    {
        float discountPercent = ShopManager.Instance.playerDiscount; 
        float newTotal = (initialCost * (quantity + 1)) * (1 - discountPercent / 100f);
        if(CoinManager.Instance.Coins >= newTotal)
        {
            quantity++;
            currentCost = newTotal;
        }
    }

    public void Remove()
    {
        if (quantity > 1)
        {
            quantity--;
            float discountPercent = ShopManager.Instance.playerDiscount;
            currentCost = (initialCost * quantity) * (1 - discountPercent / 100f);
            if (currentCost < 0) currentCost = 0;
        }
    }

    public void ApplyDiscount(float discountPercent)
    {
        currentCost = (initialCost * quantity) * (1 - discountPercent / 100f);
        if (currentCost < 0) currentCost = 0;
    }
}
