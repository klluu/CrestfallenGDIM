using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float walkSpeed = 1000;
    private float jumpVelocity = 1500;
    private float jumpWaitTime = 0.05f;
    private float jumpWaitTimer;
    public Rigidbody2D rb;

    public LayerMask ground;
    public Collider2D footCollider;

    private bool isGrounded;

    private Vector3 respawnPoint;
    public GameObject fallDetector;


    void Start()
    {
        respawnPoint = transform.position;
    }

    
    void Update()
    {
        isGrounded = footCollider.IsTouchingLayers(ground);
        Walking();
        Jumping();

        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);
    }

    void Walking()
    {
        float direction = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(walkSpeed * direction * Time.fixedDeltaTime, rb.velocity.y);

        if (direction != 0f)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y);

        }
    }

    void Jumping()
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

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "FallDetector")
        {
            transform.position = respawnPoint;
        }
        if (col.tag == "Checkpoint")
        {
            Debug.Log("CHECKPOINT");
            respawnPoint = col.transform.position;
        }
    }
}
