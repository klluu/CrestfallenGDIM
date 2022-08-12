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
    public Transform player;

    // Movement Variables
    private bool mustPatrol;
    private float speed = 15f;
    private float distance = 10f;
    [SerializeField] private float contactDamage = 100f;


    // Ground Detection
    public Transform groundDetection;
    public LayerMask groundLayer;
    private bool isGrounded;
    public Collider2D bossCollider;

    // Range + Jump
    public float range;
    private float distToPlayer;
    private bool canJump;
    [SerializeField] float jumpHeight;
    private float timeJump = 3f;
 

    void Start()
    {
        mustPatrol = true;
        canJump = true;

        _entity.onDeath += Death;
    }


    void FixedUpdate()
    {
        isGrounded = bossCollider.IsTouchingLayers(groundLayer);
    }

    void Update()
    {
        if (mustPatrol)
        {
            Patrolling();
        }
        

        
        distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= range)
        {
            if (player.position.x > transform.position.x && transform.localScale.x < 0 ||
            player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }

            
            mustPatrol = false;
            _rb.velocity = new Vector2(0, _rb.velocity.y) + _entity.KnockBack;

            if (canJump)
            {
                Debug.Log("in range can jump");
                StartCoroutine(JumpAttack());
            }
        }
        else
        {
            mustPatrol = true;
        }
    }

    void Patrolling()
    {
        Vector2 moveDir = Vector2.right * speed;
        _rb.velocity = new Vector2(moveDir.x, _rb.velocity.y) + _entity.KnockBack;

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false && canJump)
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

    IEnumerator JumpAttack()
    {
        canJump = false;
        float distanceFromPlayer = player.position.x - transform.position.x;
        Debug.Log(distanceFromPlayer);

        if (isGrounded)
        {
            Debug.Log("JUMP");
            _rb.AddForce(new Vector2(distanceFromPlayer, jumpHeight), ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(timeJump);

        canJump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EntityManager entity = collision.gameObject.GetComponent<EntityManager>();

            entity.TakeDamage(contactDamage);
        }
        if  (collision.gameObject.CompareTag("Wall"))
        {
            Flip();
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}