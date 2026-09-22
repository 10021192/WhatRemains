using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionAttack : FSMAction
{
    [Header("Config")]
    [SerializeField] private int minDamage;
    [SerializeField] private int maxDamage;
    [SerializeField] private float timeBtwAttacks;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    
    private EnemyBrain enemyBrain;
    private EnemyHealth enemyHealth;

    private float timer;

    private void Awake()
    {
        enemyBrain = GetComponent<EnemyBrain>();
        enemyHealth = GetComponent<EnemyHealth>();
    }

    public override void Act()
    {
        if (enemyBrain.IsBoss)
        {
            BossAttack();
        }
        else
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        if (enemyBrain.Player == null) return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            int damage = Random.Range(minDamage, maxDamage + 1);
            IDamageable player = enemyBrain.Player.GetComponent<IDamageable>();
            player.TakeDamage(damage);
            timer = timeBtwAttacks;
        }
    }

    private void BossAttack()
    {
        if (enemyBrain.Player == null) return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            enemyHealth.SetAttackAnimation(true);
            timer = timeBtwAttacks;
        }
    }

    public void PerformAttack()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            Vector3 direction = (enemyBrain.Player.position - projectileSpawnPoint.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            Projectile projScript = projectile.GetComponent<Projectile>();
            if (projScript != null)
            {
                projScript.Direction = direction;
                projScript.Damage = Random.Range(minDamage, maxDamage + 1);
            }
        }
        enemyHealth.IsAttacking = false;
    }
}
