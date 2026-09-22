using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public static event Action onEnemyDeadEvent;

    [Header("Config")]
    [SerializeField] private int health;

    public int CurrentHealth { get; private set; }

    private Animator animator;
    private Rigidbody2D rb2D;
    private EnemyBrain enemyBrain;
    private EnemyLoot enemyLoot;
    private EnemySelector enemySelector;

    [Header("Respawner")]
    [SerializeField] private float respawnTime;

    private Vector3 initialPosition;
    private bool isRespawning = false;

    private bool canPlayHitAnimation = true;
    private float hitAnimationCooldown = 1.8f;
    private float hitCooldownTimer = 0f;
    [HideInInspector] public bool IsAttacking = false;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyLoot = GetComponent<EnemyLoot>();
        enemyBrain = GetComponent<EnemyBrain>();
        enemySelector = GetComponent<EnemySelector>();
    }

    private void Start()
    {
        CurrentHealth = health;
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (!canPlayHitAnimation)
        {
            hitCooldownTimer -= Time.deltaTime;
            if (hitCooldownTimer <= 0)
            {
                canPlayHitAnimation = true;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;
        if(CurrentHealth <= 0)
        {
            DisableEnemy();
            QuestManager.Instance.AddProgress("Kill2Enemy", 1);
            QuestManager.Instance.AddProgress("Kill5Enemy", 1);
            QuestManager.Instance.AddProgress("Kill10Enemy", 1);
            if(enemyBrain.IsBoss)
            {
                QuestManager.Instance.AddProgress("GreatToadSage", 1);
                GameManager.IsBossDead = true;
            }
        }
        else
        {
            DamageManager.Instance.ShowDamageText(amount, transform);
            if(enemyBrain.IsBoss && !IsAttacking && canPlayHitAnimation)
            {
                SetHitAnimation();
                canPlayHitAnimation = false;
                hitCooldownTimer = hitAnimationCooldown;
            }
        }
    }

    private void DisableEnemy()
    {
        animator.SetTrigger("Dead");
        enemyBrain.enabled = false;
        enemySelector.NoSelectionCallback();
        rb2D.bodyType = RigidbodyType2D.Static;
        onEnemyDeadEvent?.Invoke();
        if(!enemyBrain.IsBoss)
            GameManager.Instance.AddPlayerExp(enemyLoot.ExpDrop);

        if (!isRespawning)
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    private void RespawnEnemy()
    {
        enemyLoot.ClearLoot();
        enemyLoot.LoadDropItems();

        transform.position = initialPosition;
        CurrentHealth = health;
        animator.ResetTrigger("Dead");
        animator.SetTrigger("Alive");

        rb2D.bodyType = RigidbodyType2D.Dynamic;
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = false;

        enemyBrain.enabled = true;
        enemyBrain.ChangeState(enemyBrain.InitState);
        isRespawning = false;
    }

    public void SetAttackAnimation(bool value)
    {
        IsAttacking = value;
        animator.SetBool("Attacking", value);
    }

    public void SetHitAnimation()
    {
        animator.ResetTrigger("Hit");
        animator.SetTrigger("Hit");
    }

    private IEnumerator RespawnCoroutine()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnTime);

        RespawnEnemy();
    }
}
