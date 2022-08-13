using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{

    // RangedEnemy Controller Essentials
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private EntityManager _entity;
    [SerializeField]
    private Collider2D _col;


    private bool mustPatrol;
    private bool canShoot;

    // Movement Variables
    private float speed = 7.5f;
    private float distance = 10f;
    private float downCheckDistance = 0;
    private float flipCheckDistance = 3f;
    private float airTime = 0;
    private float flipCD = 0;
    private float flipTime = .5f;

    // Ground Detection
    public Transform groundDetection;
    public LayerMask groundLayer;
    public Rigidbody2D rb;


    // Shooting Projectile
    public Transform player;
    public Transform shootPos;

    public float range;
    private float distToPlayer;
    private float timeShoot = 6f;
    private float shootSpeed = 20f;

    public GameObject bullet;

    void Start()
    {
        mustPatrol = true;
        canShoot = true;

        _entity.onDeath += Death;
    }


    void FixedUpdate()
    {
        GroundCheck();

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

            _rb.velocity = new Vector2(_rb.velocity.x * .99f, _rb.velocity.y);

            if (canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
        else
        {
            mustPatrol = true;
        }
    }

    private void Patrolling()
    {
        Vector2 moveDir = Vector2.right * speed;

        float addMove = moveDir.x * Time.fixedDeltaTime;
        addMove *= 1 - Mathf.Clamp(_rb.velocity.x / speed, 0, 1);

        _rb.velocity = new Vector2(_rb.velocity.x + addMove, _rb.velocity.y);
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

    IEnumerator Shoot()
    {
        canShoot = false;

        GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);

        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * Mathf.Sign(speed), 0);
        yield return new WaitForSeconds(timeShoot);

        canShoot = true;
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