using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    // Controller Essentials
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private EntityManager _entity;

    // Movement Variables
    private bool mustPatrol;
    private float speed = 10;
    private float distance = 10f;
    [SerializeField] private float contactDamage = 100f;


    // Ground Detection
    public Transform groundDetection;
    public LayerMask groundLayer;


    void Start()
    {
        mustPatrol = true;

        _entity.onDeath += Death;
    }


    void Update()
    {
        if (mustPatrol)
        {
            Patrolling();
        }

    }

    void Patrolling()
    {
        Vector2 moveDir = Vector2.right * speed;
        _rb.velocity = new Vector2(moveDir.x, _rb.velocity.y) + _entity.KnockBack;

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false)
        {
            Flip();
        }

    }

    void Flip()
    {
        mustPatrol = false;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        speed *= -1;
        mustPatrol = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EntityManager entity = collision.gameObject.GetComponent<EntityManager>();

            entity.TakeDamage(contactDamage);
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}