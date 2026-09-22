using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySelector : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private GameObject selectorSprite;

    private EnemyBrain EnemyBrain;

    private void Awake()
    {
        EnemyBrain = GetComponent<EnemyBrain>();
    }

    public void EnemySelectedCallback(EnemyBrain enemySelected)
    {
        if (enemySelected == EnemyBrain)
        {
            selectorSprite.SetActive(true);
        }
        else
        {
            selectorSprite.SetActive(false);
        }
    }

    public void NoSelectionCallback()
    {
        selectorSprite.SetActive(false );
    }

    private void OnEnable()
    {
        SelectionManager.OnEnemySelectedEvent += EnemySelectedCallback;
        SelectionManager.OnNoSelectionEvent += NoSelectionCallback;
    }

    private void OnDisable()
    {
        SelectionManager.OnEnemySelectedEvent -= EnemySelectedCallback;
        SelectionManager.OnNoSelectionEvent -= NoSelectionCallback;
    }
}
