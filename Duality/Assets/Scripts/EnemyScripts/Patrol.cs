using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{

    // Controller Essentials
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private EntityManager _entity;

    // Movement Variables
    private bool mustPatrol;
    private float speed = 10f;
    private float distance = 10f;
    

    // Ground Detection
    public Transform groundDetection;
    public LayerMask groundLayer;


    void Start()
    {
        mustPatrol = true;
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
        transform.Translate(Vector2.right * speed * Time.deltaTime);

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

}
