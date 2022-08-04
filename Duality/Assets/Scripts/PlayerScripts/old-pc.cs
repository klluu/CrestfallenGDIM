using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Controller Essentials
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private EntityManager _entity;

    // Movement Variables
    [SerializeField]
    private LayerMask mapMask;
    [SerializeField]
    private LayerMask enemyMask;
    private Vector2 _moveDirection = Vector2.zero;
    private float _moveSpeed = 25f;
    private float _jumpPower = 800f;
    private float _ungroundable = 0f;
    private bool _grounded = true;

    private bool GroundCheck()
    {
        if (Time.time > _ungroundable)
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, .9f, Vector2.down, 1.5f, mapMask);

            if (hit.collider != null)
            {
                //transform.position = new Vector2(transform.position.x, hit.point.y + 1.5f);

                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * _moveSpeed;
        _moveDirection = new Vector2(moveX, _rb.velocity.y);

        bool jumped = Input.GetButton("Jump");
        bool attack = Input.GetMouseButton(0);

        if (jumped && _grounded)
        {
            _moveDirection.y = _jumpPower *10;
            _ungroundable = Time.time + .1f;
        }

        if (attack)
        {
            RaycastHit2D hitResult = Physics2D.BoxCast(transform.position, new Vector2(2, 3), 0, transform.forward, 10f, enemyMask);

            if (hitResult && hitResult.transform.CompareTag("Enemy"))
            {
                hitResult.transform.GetComponent<EntityManager>().TakeDamage(1f);
            }
        }

        _rb.velocity = _moveDirection;
    }

    private void FixedUpdate()
    {
        _grounded = GroundCheck();
    }

}
