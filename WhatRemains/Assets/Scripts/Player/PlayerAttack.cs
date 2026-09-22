using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAttack : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Weapon initialWeapon;
    [SerializeField] private Transform[] attackPositions;

    [Header("Melee Config")]
    [SerializeField] private ParticleSystem slashFX;
    [SerializeField] private float minDistanceMeleeAttack;

    [Header("Attack Cooldown")]
    [SerializeField] private float attackCooldown;
    private bool isOnCooldown = false;

    public Weapon CurrentWeapon { get; set; }

    private PlayerActions actions;
    private PlayerAnimations playerAnimations;
    private PlayerMovement playerMovement;
    private PlayerMana playerMana;
    private EnemyBrain enemyTarget;
    private Coroutine attackCoroutine;

    private Transform currentAttackPosition;
    private float currentAttackRotation;

    private void Awake()
    {
        actions = new PlayerActions();
        playerMana = GetComponent<PlayerMana>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimations = GetComponent<PlayerAnimations>();
    }

    private void Start()
    {
        WeaponManager.Instance.EquipWeapon(initialWeapon);
        actions.Attack.ClickAttack.performed += ctx => Attack();
    }

    private void Update()
    {
        GetFirePosition();
    }

    private void Attack()
    {
        if(!PauseMenu.isPaused)
        {
            if(enemyTarget == null || isOnCooldown) return;
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            attackCoroutine = StartCoroutine(IEAttack());
        } 
    }

    private IEnumerator IEAttack()
    {
        if(currentAttackPosition == null) yield break;

        isOnCooldown = true;
        if(CurrentWeapon.WeaponType == WeaponType.Magic)
        {
            if(playerMana.CurrentMana < CurrentWeapon.RequiredMana)
            {
                isOnCooldown = false;
                yield break;
            }
            MagicAttack();
        }
        else
        {
            MeleeAttack();
        }

        playerAnimations.SetAttackAnimation(true);
        yield return new WaitForSeconds(0.15f);
        playerAnimations.SetAttackAnimation(false);

        yield return new WaitForSeconds(attackCooldown);
        isOnCooldown = false;
    }

    private void MagicAttack()
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0f, 0f, currentAttackRotation));
        Projectile projectile = Instantiate(CurrentWeapon.ProjectilePrefab, currentAttackPosition.position, rotation);
        projectile.Direction = Vector3.up;
        projectile.Damage = GetAttackDamage();
        playerMana.UseMana(CurrentWeapon.RequiredMana);
    }

    private void MeleeAttack()
    {
        slashFX.transform.position = currentAttackPosition.position;
        slashFX.Play();
        float currentDistanceToEnemy = Vector3.Distance(enemyTarget.transform.position, transform.position);
        if(currentDistanceToEnemy <= minDistanceMeleeAttack)
        {
            enemyTarget.GetComponent<IDamageable>().TakeDamage(GetAttackDamage());
        }
    }

    public void EquipWeapon(Weapon newWeapon)
    {
        CurrentWeapon = newWeapon;
        stats.TotalDamage = stats.BaseDamage + CurrentWeapon.Damage;
    }

    private int GetAttackDamage()
    {
        int damage = stats.BaseDamage;
        damage += CurrentWeapon.Damage;
        float randomPerc = Random.Range(0f, 100f);
        if(randomPerc <= stats.CriticalChance)
        {
            damage += Mathf.RoundToInt(damage * (stats.CriticalDamage / 100f));
        }
        return damage;
    }

    private void GetFirePosition()
    {
        Vector2 moveDirection = playerMovement.MoveDirection;
        switch(moveDirection.x)
        {
            case > 0f:
                currentAttackPosition = attackPositions[1];
                currentAttackRotation = -90f;
            break;
            case < 0f:
                currentAttackPosition = attackPositions[3];
                currentAttackRotation = -270f;
            break;
        }

        switch(moveDirection.y)
        {
            case > 0f:
                currentAttackPosition = attackPositions[0];
                currentAttackRotation = 0f;
            break;
            case < 0f:
                currentAttackPosition = attackPositions[2];
                currentAttackRotation = 180f;
            break;
        }
    }

    private void EnemySelectedCallback(EnemyBrain enemySelected)
    {
        enemyTarget = enemySelected;
    }

    private void NoEnemySelectionCallback()
    {
        enemyTarget = null;
    }

    private void OnEnable()
    {
        actions.Enable();
        SelectionManager.OnEnemySelectedEvent += EnemySelectedCallback;
        SelectionManager.OnNoSelectionEvent += NoEnemySelectionCallback;
        EnemyHealth.onEnemyDeadEvent += NoEnemySelectionCallback;
    }

    private void OnDisable()
    {
        actions.Disable();
        SelectionManager.OnEnemySelectedEvent -= EnemySelectedCallback;
        SelectionManager.OnNoSelectionEvent -= NoEnemySelectionCallback;
        EnemyHealth.onEnemyDeadEvent -= NoEnemySelectionCallback;
    }
}
