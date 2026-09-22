using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private TextMeshProUGUI damageTMP;

    public void SetDamageText(int damage)
    {
        damageTMP.text = damage.ToString();
    }

    public void DestroyText()
    {
        Destroy(gameObject);
    }
}
