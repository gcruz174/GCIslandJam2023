using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public bool isDead = false;
    
    public GameObject damageEffect;

    public event Action OnDamage;
    public event Action OnDeath;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0f)
        {
            isDead = true;
            currentHealth = 0f;
        }
        
        if (damageEffect != null)
            Instantiate(damageEffect, transform.position, Quaternion.identity);
    }

    public void Heal(float heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
    }
}
