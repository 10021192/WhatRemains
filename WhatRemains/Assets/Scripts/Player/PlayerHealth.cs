using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    private PlayerAnimations playerAnimations;

    [HideInInspector] public bool IsRegenActive = false;

    public bool godMode = false;

    private void Awake()
    {
        playerAnimations = GetComponent<PlayerAnimations>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            godMode = !godMode;
            Debug.Log("God Mode: " + godMode);
        }

        if(stats.Health <= 0f) PlayerDead();
    }

    public void TakeDamage(int amount)
    {
        if(godMode) return;

        if(stats.Health <= 0) return;
        stats.Health -= amount;
        DamageManager.Instance.ShowDamageText(amount, transform);
        if(stats.Health <= 0)
        {
            stats.Health = 0;
            PlayerDead();
        }
    }

    private void ClampHealth()
    {
        if(stats.Health > stats.MaxHealth) stats.Health = stats.MaxHealth;
    }

    public bool CanRestoreHealth()
    {
        return stats.Health > 0 && stats.Health < stats.MaxHealth;
    }

    public void RestoreHealth(int amount)
    {
        stats.Health += amount;
        ClampHealth();
    }
    public void ResetHealth()
    {
        stats.Health = stats.MaxHealth;
    }
    
    public void StartHealthRegen(int regenAmount, int regenInterval, int duration)
    {
        if (IsRegenActive)
        {
            Debug.Log("Health regeneration is already active.");
            return;
        }
        IsRegenActive = true;
        StartCoroutine(HealthRegenCoroutine(regenAmount, regenInterval, duration));
    }

    private IEnumerator HealthRegenCoroutine(int regenAmount, int regenInterval, int duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            if (CanRestoreHealth())
            {
                stats.Health += regenAmount;
                ClampHealth();
            }
            yield return new WaitForSeconds(regenInterval);
            elapsedTime += regenInterval;
        }
        IsRegenActive = false;
        Debug.Log("Health regeneration effect has ended.");
    }

    private void PlayerDead()
    {
        playerAnimations.SetDeadAnimation();
        GameManager.Instance.gameOverMenu.ShowGameOverMenu();
    }
}
