using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] public HealthBarController healthBarController;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private float speed;
    [SerializeField] private float dir = 1;
    [SerializeField] public Transform startPoint;
    [SerializeField] public Transform endPoint;
    [SerializeField] public Transform playerProximityPoint;
    [SerializeField] public float playerProximityPointRadius;
    [SerializeField] public LayerMask playerLayerMask;

    private Animator anim;
    private Rigidbody2D enemyRb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Move();
        ProximityCheck();
    }


    public void Move()
    {
        anim.SetFloat("hSpeed", Mathf.Abs(enemyRb.linearVelocity.x));
        if (transform.position.x < startPoint.position.x)
        {
            dir = 1;

            transform.localScale = new Vector3(0.611090004f, 0.611090004f, 0.611090004f);
        }
        if (transform.position.x > endPoint.position.x)
        {
            dir = -1;
            transform.localScale = new Vector3(-0.611090004f, 0.611090004f, 0.611090004f);
            
        }
        enemyRb.linearVelocity = new Vector2(dir * speed, enemyRb.linearVelocity.y);
    }

    
    public void Flip()
    {
        
    }


    public void TakeDamage(float inputDamage)
    {
        currentHealth -= inputDamage;
        healthBarController.RefreshHealthBar(currentHealth, maxHealth);
        if (currentHealth < 0)
        {
            Death();
        }
    }


    //проверка наличия игрока в радиусе и остановка
    public void ProximityCheck()
    {
        bool collider = Physics2D.OverlapCircle(playerProximityPoint.position, playerProximityPointRadius, playerLayerMask);
        if (collider)
        {
            collider = false;
            enemyRb.linearVelocity = Vector2.zero;
        }

    }


    public void Death()
    {
        Destroy(gameObject);
    }


    private void OnDrawGizmosSelected()
    {
        if (!playerProximityPoint) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(playerProximityPoint.position, playerProximityPointRadius);
    }
}
