using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    // Entity Essentials
    public delegate void OnDeath();
    public OnDeath onDeath;

    public delegate void OnDamage(float healthDif);
    public OnDamage onDamage;

    // Stats Variables
    public float MaxHealth = 100;
    public float Health;

    // Movement Variables
    public bool KnockBack = false;

    private void Awake()
    {
        Health = MaxHealth;

        onDeath += Dead;
    }

    public float TakeDamage(float damage)
    {
        float prevHealth = Health;
        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
            onDeath?.Invoke();
        }
        else
        {
            onDamage?.Invoke(prevHealth - Health);
        }

        return Health;
    }

    private void Dead()
    {
        // Play Dead Animation?
        //Destroy(gameObject);
    }
}