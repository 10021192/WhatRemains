using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    public static event Action OnPlayerUpgradeEvent;

    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    [Header("Settings")]
    [SerializeField] private UpgradeSettings[] settings;

    [HideInInspector] public bool isBoostActive = false;

    private int preBoostHealth;
    private int preBoostMana;

    public void ApplyTemporaryBoost(AttributeType attributeType, float duration)
    {
        if (isBoostActive)
        {
            Debug.Log("Boost is already active. Wait until it wears off before using another cookie.");
            return;
        }
        isBoostActive = true;
        
        UpgradeAttributes(attributeType);
        OnPlayerUpgradeEvent?.Invoke();
        StartCoroutine(RevertTemporaryBoost(attributeType, duration));
    }

    private void ApplyAttributeBoost(int upgradeIndex)
    {
        preBoostHealth = stats.Health;
        preBoostMana = stats.Mana;

        stats.BaseDamage += settings[upgradeIndex].DamageUpgrade;
        stats.TotalDamage += settings[upgradeIndex].DamageUpgrade;
        stats.MaxHealth += settings[upgradeIndex].HealthUpgrade;
        int healthIncrease = (stats.MaxHealth - stats.Health) / 2;
        stats.Health += healthIncrease;
        stats.MaxMana += settings[upgradeIndex].ManaUpgrade;
        int manaIncrease = (stats.MaxMana - stats.Mana) / 2;
        stats.Mana += manaIncrease;
        stats.CriticalChance += settings[upgradeIndex].CChanceUpgrade;
        stats.CriticalDamage += settings[upgradeIndex].CDamageUpgrade;
    }

    private void RevertAttributeBoost(int attributeIndex)
    {
        stats.BaseDamage -= settings[attributeIndex].DamageUpgrade;
        stats.TotalDamage -= settings[attributeIndex].DamageUpgrade;
        stats.MaxHealth -= settings[attributeIndex].HealthUpgrade;
        stats.Health = Math.Min(preBoostHealth, stats.MaxHealth);
        stats.MaxMana -= settings[attributeIndex].ManaUpgrade;
        stats.Mana = Math.Min(preBoostMana, stats.MaxMana);
        stats.CriticalChance -= settings[attributeIndex].CChanceUpgrade;
        stats.CriticalDamage -= settings[attributeIndex].CDamageUpgrade;
    }

    private void AttributeCallback(AttributeType attributeType)
    {
        if(stats.AttributePoints == 0) return;
        UpgradeAttributes(attributeType);

        stats.AttributePoints--;
        OnPlayerUpgradeEvent?.Invoke();
    }

    private void UpgradeAttributes(AttributeType attributeType)
    {
        switch(attributeType)
        {
            case AttributeType.Strength:
                ApplyAttributeBoost(0);
                stats.Strength++;
            break;
            case AttributeType.Dexterity:
                ApplyAttributeBoost(1);
                stats.Dexterity++;
            break;
            case AttributeType.Intelligence:
                ApplyAttributeBoost(2);
                stats.Intelligence++;
            break;
        }
    }

    private IEnumerator RevertTemporaryBoost(AttributeType attributeType, float duration)
    {
        yield return new WaitForSeconds(duration);

        switch (attributeType)
        {
            case AttributeType.Strength:
                RevertAttributeBoost(0);
                stats.Strength--;
            break;
            case AttributeType.Dexterity:
                RevertAttributeBoost(1);
                stats.Dexterity--;
            break;
            case AttributeType.Intelligence:
                RevertAttributeBoost(2);
                stats.Intelligence--;
            break;
        }
        isBoostActive = false;
    }

    private void OnEnable()
    {
        AttributeButton.OnAttributeSelectedEvent += AttributeCallback;
        OnPlayerUpgradeEvent += UIManager.Instance.UpgradeCallback;
    }

    private void OnDisable()
    {
        AttributeButton.OnAttributeSelectedEvent -= AttributeCallback;
        OnPlayerUpgradeEvent -= UIManager.Instance.UpgradeCallback;
    }
}

[Serializable]
public class UpgradeSettings
{
    public string Name;

    [Header("Values")]
    public int DamageUpgrade;
    public int HealthUpgrade;
    public int ManaUpgrade;
    public float CChanceUpgrade;
    public int CDamageUpgrade;
}