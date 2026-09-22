using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;

    public PlayerStats Stats => stats;
    public PlayerMana PlayerMana { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerAttack PlayerAttack { get; private set; }
    public PlayerUpgrade PlayerUpgrade { get; private set; }

    private PlayerAnimations animations;

    private void Awake()
    {
        PlayerMana = GetComponent<PlayerMana>();
        PlayerHealth = GetComponent<PlayerHealth>();
        PlayerAttack = GetComponent<PlayerAttack>();
        PlayerUpgrade = GetComponent<PlayerUpgrade>();
        animations = GetComponent<PlayerAnimations>();
    }

    public void ResetPlayer()
    {
        stats.ResetPlayer();
        animations.ResetPlayer();
        PlayerMana.ResetMana();
    }
}
