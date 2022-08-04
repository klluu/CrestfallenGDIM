using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private float dieTime = 2f;
    public float damage;

    void Start()
    {
        StartCoroutine(CountDownTimer());
    }

    void OnCollisionEnter2D(Collision2D col)
    {
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
