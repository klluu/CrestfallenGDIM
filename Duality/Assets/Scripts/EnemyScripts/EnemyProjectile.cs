using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private float dieTime = 2f;
    private float damage = 100f;

    void Start()
    {
        StartCoroutine(CountDownTimer());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            EntityManager entity = col.gameObject.GetComponent<EntityManager>();

            entity.TakeDamage(damage);
        }

        Die();
    }

    IEnumerator CountDownTimer()
    {
        yield return new WaitForSeconds(dieTime);

        Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
