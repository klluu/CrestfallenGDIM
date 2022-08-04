using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // Player Controller Essentials
    [SerializeField]
    public Rigidbody2D rb;
    [SerializeField]
    private EntityManager _entity;
    [SerializeField]
    private LayerMask _enemyLayer;

    private float walkSpeed = 1000;
    private float jumpVelocity = 1500;
    private float jumpWaitTime = 0.05f;
    private float jumpWaitTimer;

    public LayerMask ground;
    public Collider2D footCollider;

    private bool isGrounded;

    private Vector3 respawnPoint;
    public GameObject fallDetector;

    [SerializeField] private float _damage = 100f;
    [SerializeField] private float _attackRange = 5f;


    void Start()
    {
        respawnPoint = transform.position;
        _entity.onDeath += Respawn;
    }

    
    void Update()
    {
        isGrounded = footCollider.IsTouchingLayers(ground);
        Walking();
        Jumping();
        Attacking();

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void Walking()
    {
        float direction = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(walkSpeed * direction * Time.fixedDeltaTime, rb.velocity.y);

        if (direction != 0f)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y);

        }
    }

    private void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || jumpWaitTimer > 0f)
            {
                Jump();
            }
        }

        //Timer
        if (isGrounded)
        {
            jumpWaitTimer = jumpWaitTime;
        }
        else
        {
            if (jumpWaitTimer > 0f)
            {
                jumpWaitTimer -= Time.deltaTime;
            }
        }
    }

    private void Attacking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hitResult = Physics2D.BoxCast(transform.position, new Vector2(5, 5), 0, transform.right, _attackRange * Mathf.Sign(Input.GetAxisRaw("Horizontal")), _enemyLayer);

            if (hitResult && hitResult.transform.CompareTag("Enemy"))
            {
                hitResult.transform.GetComponent<EntityManager>().TakeDamage(_damage);
            }
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "FallDetector")
        {
            _entity.TakeDamage(_entity.MaxHealth);
        }
        if (col.tag == "Checkpoint")
        {
            Debug.Log("CHECKPOINT");
            respawnPoint = col.transform.position;
        }
    }

    private void Respawn()
    {
        _entity.Health = 100f;
        transform.position = respawnPoint;
    }
}