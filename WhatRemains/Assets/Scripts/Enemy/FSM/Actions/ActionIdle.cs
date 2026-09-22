using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIdle : FSMAction
{
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    public override void Act()
    {
        enemyHealth.SetAttackAnimation(false);
    }
}
