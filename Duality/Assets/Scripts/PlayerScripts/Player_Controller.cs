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
    [SerializeField]
    private GameObject _character;

    // Player Stats
    private float walkSpeed = 20;
    private float jumpVelocity = 35;
    private float jumpWaitTime = 0.05f;
    private float jumpWaitTimer;

    // Ground Variables
    public LayerMask ground;
    public Collider2D footCollider;

    private bool isGrounded;

    // Checkpoints
    private Vector3 respawnPoint;
    public GameObject fallDetector;


    void Start()
    {
        respawnPoint = transform.position;
        _entity.onDeath += Respawn;
    }


    void FixedUpdate()
    {
        isGrounded = footCollider.IsTouchingLayers(ground);
        Walking();

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || jumpWaitTimer > 0f)
            {
                Jump();
            }
        }

        if (isGrounded)
        {
            jumpWaitTimer = jumpWaitTime;
        }
        else
        {
            if (jumpWaitTimer > 0f)
            {
                jumpWaitTimer -= Time.fixedDeltaTime;
            }
        }
    }

    private void Walking()
    {
        float direction = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(walkSpeed * direction, rb.velocity.y) + _entity.KnockBack;

        if (direction != 0f)
        {
            _character.transform.localScale = new Vector2(Mathf.Abs(_character.transform.localScale.x) * direction, _character.transform.localScale.y);

        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
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