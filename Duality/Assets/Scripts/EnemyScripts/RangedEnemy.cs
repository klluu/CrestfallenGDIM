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


    private bool mustPatrol;
    private bool canShoot;
    
    // Movement Variables
    private float speed = 10f;
    private float distance = 10f;
    
    // Ground Detection
    public Transform groundDetection;
    public LayerMask groundLayer;
    public Rigidbody2D rb;

    
    // Shooting Projectile
    public Transform player;
    public Transform shootPos;

    public float range;
    private float distToPlayer;
    private float timeShoot = 2f;
    private float shootSpeed = 100f;

    public GameObject bullet;

    void Start()
    {
        mustPatrol = true;
        canShoot = true;
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
            if(player.position.x > transform.position.x && transform.localScale.x < 0 || 
            player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }

            mustPatrol = false;
            rb.velocity = Vector2.zero;

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

    IEnumerator Shoot()
    {
        canShoot = false;
        
        GameObject newBullet = Instantiate(bullet, shootPos.position, Quaternion.identity);

        newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(shootSpeed * speed * Time.fixedDeltaTime, 0);
        yield return new WaitForSeconds(timeShoot);
        
        canShoot = true;
    }
}