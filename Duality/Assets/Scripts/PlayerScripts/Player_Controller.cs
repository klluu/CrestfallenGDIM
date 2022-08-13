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

    private bool isGrounded;

    // Checkpoints
    private Vector3 respawnPoint;
    public GameObject fallDetector;

    // Check Direction
    private bool facingRight = true;

    void Start()
    {
        respawnPoint = transform.position;
        _entity.onDeath += Respawn;
    }


    void FixedUpdate()
    {
        isGrounded = GroundCheck();
        Walking();

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded || jumpWaitTimer > 0f)
            {
                print("dog");
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

        rb.velocity = new Vector2(walkSpeed * direction, rb.velocity.y);

        /***
        if (direction != 0f)
        {
            _character.transform.localScale = new Vector2(Mathf.Abs(_character.transform.localScale.x) * direction, _character.transform.localScale.y);

        }
        ***/

        
        //change direction
        if (facingRight && direction < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        else if (!facingRight && direction > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
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

    private bool GroundCheck()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(transform.position, Vector2.down, 9.8f, ground);

        if (groundInfo)
        {
            return true;
        }

        return false;
    }
}