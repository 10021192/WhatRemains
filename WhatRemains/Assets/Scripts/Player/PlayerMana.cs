using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    public float CurrentMana { get; private set; }

    [HideInInspector] public bool IsRegenActive = false;

    private void Start()
    {
        ResetMana();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.M)) UseMana(1);
    }

    public void UseMana(int amount)
    {
        stats.Mana = Math.Max(stats.Mana - amount, 0);
        CurrentMana = stats.Mana;
    }


    private void ClampMana()
    {
        if (stats.Mana >= stats.MaxMana)
        {
            stats.Mana = stats.MaxMana;
            CurrentMana = stats.Mana;
            return;
        }
        CurrentMana = stats.Mana;
    }

    public bool CanRecoverMana()
    {
        return stats.Health > 0 && stats.Mana < stats.MaxMana;
    }

    public void RecoverMana(int amount)
    {
        stats.Mana += amount;
        ClampMana();
    }

    public void StartManaRegen(int regenAmount, int regenInterval, int duration)
    {
        if (IsRegenActive)
        {
            Debug.Log("Mana regeneration is already active.");
            return;
        }
        IsRegenActive = true;
        StartCoroutine(ManaRegenCoroutine(regenAmount, regenInterval, duration));
    }

    private IEnumerator ManaRegenCoroutine(int regenAmount, int regenInterval, int duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            if (CanRecoverMana())
            {
                stats.Mana += regenAmount;
                ClampMana();
            }
            yield return new WaitForSeconds(regenInterval);
            elapsedTime += regenInterval;
        }
        IsRegenActive = false;
        Debug.Log("Mana regeneration effect has ended.");
    }

    public void ResetMana()
    {
        CurrentMana = stats.MaxMana;
    }
}
