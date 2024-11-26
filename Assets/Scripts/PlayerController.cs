using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D playerRb;
    private bool isGrounded;
    public bool attack;
    private bool isDead;
    [Header ("Health")]
    [SerializeField] public HealthBarController healthBarController;
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;

    [Header("Movement")]
    [SerializeField] private Vector2 dir;
    [SerializeField] private float speed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private float groundPointRadius;

    [Header("Fight")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackPointRadius;
    [SerializeField] private float damage;
    [SerializeField] private float attackDelay;
    [SerializeField] private float attackCooldown;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        if (isDead) return;
        Move();

    }


    public void Move()
    {
        playerRb.linearVelocity = new Vector2(dir.x * speed, playerRb.linearVelocity.y);
        anim.SetFloat("hSpeed", Mathf.Abs(playerRb.linearVelocity.x));
        Flip();
        Jump();
    }


    private void Flip()
    {
        if (dir.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (dir.x > 0) 
        {
            transform.localScale = Vector3.one;
        }
    }


    public void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, groundPointRadius, groundLayer);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("vSpeed", playerRb.linearVelocity.y);
    }

    private void OnJump(InputValue value)
    {
        
        if (isGrounded)
        {
            playerRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        
    }

    //получение урона
    public void TakeDamage(float inputDamage)
    {
        currentHealth -= inputDamage;
        anim.SetTrigger("Hurt");
        healthBarController.RefreshHealthBar(currentHealth, maxHealth);
        if(currentHealth < 0)
        {
            Death();
        }
    }


    public void Death()
    {
        isDead = true;
        anim.SetBool("isDead", true);
        playerRb.bodyType = RigidbodyType2D.Kinematic;
        playerRb.linearVelocity = Vector3.zero;
        GetComponent<Collider2D>().enabled = false;
        enabled = false;
    }

    public void OnAttack(InputValue value)
    {
        if (isDead) return;// проверка isDead true
        anim.SetTrigger("Attack");
        StartCoroutine(DamageDealingDelay(attackDelay));
    }


    public void Attack()
    {
        //Создаем круг в котором противники будут получать урон
        Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackPointRadius, enemyLayer);
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            if (enemyColliders[i].GetComponent<EnemyController>())
            {
                enemyColliders[i].GetComponent<EnemyController>().TakeDamage(damage);
            }
        }
    }


    private IEnumerator DamageDealingDelay(float time)
    {
        
        yield return new WaitForSeconds(time);
        Attack();
    } 


    public void OnMove(InputValue value)
    {
        dir = value.Get<Vector2>();
    }


    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        isGrounded = true;
    }


    private void OnDrawGizmosSelected()
    {
        //создаем гизмос сферу для граундпойнт
        if (!groundPoint) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundPoint.position, groundPointRadius);
        //создаем гизмос сферу для аттакпойнт
        if(!attackPoint) return; // проверка если не объявлен аттак пойнт 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackPointRadius); //визуальное отображение круга
    }
}