using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBrute : MonoBehaviour
{

    // Controller Essentials
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private EntityManager _entity;
    [SerializeField]
    private Collider2D _col;

    // Movement Variables
    private bool mustPatrol;
    private float speed = 10;
    private float downCheckDistance = 0;
    private float flipCheckDistance = 3f;
    private float airTime = 0;
    private float flipCD = 0;
    private float flipTime = .5f;
    [SerializeField] private float contactDamage = 100f;


    // Ground Detection
    public LayerMask groundLayer;


    void Start()
    {
        mustPatrol = true;

        _entity.onDeath += Death;
    }


    void FixedUpdate()
    {
        if (GroundCheck())
        {
            airTime = 0;
        }
        else
        {
            airTime += Time.fixedDeltaTime;
        }

        if (mustPatrol)
        {
            Patrolling();
        }

    }

    private void Patrolling()
    {
        Vector2 moveDir = Vector2.right * speed;

        float addMove = moveDir.x * Time.fixedDeltaTime;
        addMove *= 1 - Mathf.Clamp(_rb.velocity.x / speed, 0, 1);

        _rb.velocity = new Vector2(_rb.velocity.x + addMove, _rb.velocity.y);
        print(_rb.velocity.x);
    }

    private void Flip()
    {
        if (flipCD > Time.time | Mathf.Abs(_rb.velocity.x) > Mathf.Abs(speed) + .1f) return;
        flipCD = Time.time + flipTime;

        _rb.velocity = new Vector2(-_rb.velocity.x, _rb.velocity.y);
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

    private bool GroundCheck()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(transform.position, Vector2.down, _col.bounds.size.y + flipCheckDistance, groundLayer);
        RaycastHit2D forwardInfo = Physics2D.BoxCast(transform.position, new Vector2(.1f, 6f), 0, Vector2.right * Mathf.Sign(speed), 2.5f, groundLayer);

        if (groundInfo)
        {
            if (groundInfo.distance > _col.bounds.size.y + downCheckDistance)
            {
                airTime += Time.fixedDeltaTime;
            }

            if (forwardInfo)
            {
                Flip();
            }

            return true;
        }
        else
        {
            if (airTime < .1f)
            {
                Flip();
            }

            airTime += Time.fixedDeltaTime;
        }

        return false;
    }
}