using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected string enemyName;
    [SerializeField] protected float enemyHealth;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float normalDamage;
    [SerializeField] protected float criticalDamage;

    [SerializeField] protected float attackRange = 3f;  // Default attack range
    [SerializeField] protected Transform transformPlayer;
    [SerializeField] protected Transform transformEnemy;
    [SerializeField] protected Vector3 originalPosition;

    [SerializeField] protected Animator animatorPlayer;
    [SerializeField] protected Animator animatorEnemy;
    [SerializeField] protected PlayerBehaviour playerBehaviour;

    [SerializeField] protected BattleHandler battleHandler;
    protected bool isMoving = false;
    protected bool isDetected = false;

    protected GameObject characterPlayer;
    protected GameObject characterEnemy;

    // Abstract methods for unique enemy behavior
    protected abstract void Attack();    // Attack behavior, unique to each enemy
    protected abstract void Attack2();    // Attack behavior, unique to each enemy
    public abstract void Move();      // Movement behavior, unique to each enemy
    protected abstract void Init();      // Initialization logic, unique to each enemy

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Init();

        if (characterPlayer == null)
        {
            characterPlayer = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            Debug.Log("Found Player Object");
        }

        if (characterEnemy == null)
        {
            characterEnemy = this.gameObject;
        }
        else
        {
            Debug.Log("Found Enemy Object");
        }

        // Initialize animators at the start
        if (animatorPlayer == null)
        {
            animatorPlayer = characterPlayer.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("Found player Animator");
        }
        if (animatorEnemy == null)
        {
            animatorEnemy = characterEnemy.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("Found enemy Animator");
        }
        if (transformPlayer == null)
        {
            transformPlayer = characterPlayer.GetComponent<Transform>();
        }
        else
        {
            Debug.Log("Found player Transform");
        }
        if (transformEnemy == null)
        {
            transformEnemy = characterEnemy.GetComponent<Transform>();
            originalPosition = transformEnemy.position;
        }
        else
        {
            Debug.Log("Found enemy Transform");
        }
       
         //Move();
    }



    public virtual void TakeDamage(float playerDamage)
    {
        enemyHealth -= playerDamage;
        if (enemyHealth <= 0)
        {
            enemyHealth = 0;
            Die();
        }
        else
        {
            StartCoroutine(EnemyDamaged());
        }
        Debug.Log($"Enemy Health: {enemyHealth}");
    }

    protected virtual IEnumerator EnemyDamaged()
    {
        animatorEnemy.SetInteger("AnimState", 5);  // Assuming 5 is the damage animation
        yield return new WaitForSeconds(0.3f);
        animatorEnemy.SetInteger("AnimState", 0);  // Back to idle
    }

    public virtual void Die()
    {
        animatorEnemy.SetInteger("AnimState", 4);  // Assuming 4 is the die animation
        Debug.Log("Enemy died!");
        StartCoroutine(DeactivateAfterDeath());
    }

    protected virtual IEnumerator DeactivateAfterDeath()
    {
        yield return new WaitForSeconds(0.9f);
        gameObject.SetActive(false);  // Deactivate the enemy
    }

   
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isDetected)
            {
                Debug.Log("Player collision detected");
                isDetected = true;
            }
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            isDetected = false;

            if (Random.Range(1, 3) == 1)
            {
                Attack();
                
            }
            else
            {
                Attack2();
                
            }
        }
    }

    public string getEnemyName()
    {
        return this.enemyName;
    }

    public float getEnemyHealth()
    {
        return this.enemyHealth;
    }

    public bool getMoving()
    {
        return this.isMoving;
    }
}
