using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class PlayerBehaviour : MonoBehaviour
{
    private Vector3 originalPosition;
    [SerializeField] Animator animatorPlayer;
    [SerializeField] Animator animatorEnemy;
    [SerializeField] Transform transformPlayer;  // The target to move toward (enemy or player)
    [SerializeField] Transform transformEnemy;
    [SerializeField] GameObject characterPlayer;
    [SerializeField] GameObject characterEnemy;
    [SerializeField] GameObject characterDie;
    [SerializeField] BattleHandler battleHandler;

    public Enemy enemyBehaviour;

    bool isDetected = false;
    public float moveSpeed = 5f;
    public float attackRange = 1f;

    public float playerHealth = 100;
    public float normalDamage = 20;
    public float criticalDamage = 20;
    public bool isMoving = false;
    public bool isCrit = false;
    private void Start()
    {
        if (characterPlayer == null)
        {
            characterPlayer = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            Debug.Log("Found playerObject");
        }
        if (characterDie == null)
        {
            characterDie = GameObject.FindGameObjectWithTag("Die");
        }
        else
        {
            Debug.Log("Found player Die");
        }

        if (characterEnemy == null)
        {
            characterEnemy = GameObject.FindGameObjectWithTag("Enemy");
        }
        else
        {
            Debug.Log("Found EnemyObject");
        }

        // Initialize animators at the start
        if (animatorPlayer == null)
        {
            animatorPlayer = characterPlayer.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("Found playerAnimator");
        }
        if (animatorEnemy == null)
        {
            animatorEnemy = characterEnemy.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("Found enemyAnimator");
        }
        if (transformPlayer == null)
        {
            transformPlayer = characterPlayer.GetComponent<Transform>();
        }
        else
        {
            Debug.Log("Found playerTransform");
        }
        if (transformEnemy == null)
        {
            transformEnemy = characterEnemy.GetComponent<Transform>();
        }
        else
        {
            Debug.Log("Found enemyTransform");
        }

    }

    public void WalkToTarget()
    {
        isMoving = true;
        // Start the coroutine to move toward the target (enemy)
        StartCoroutine(WalkToEnemy());
    }

    private IEnumerator WalkToEnemy()
    {

        originalPosition = characterPlayer.transform.position;
        Vector3 targetPosition = transformEnemy.position;

        // Start walking animation
        animatorPlayer.SetInteger("AnimState", 1);  // Walk animation

        // Move toward the enemy until within attack range
        while (Vector3.Distance(transformPlayer.position, targetPosition) > attackRange)
        {
            // Smoothly move towards the enemy
            transformPlayer.position = Vector3.MoveTowards(transformPlayer.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;  // Wait for the next frame
        }

        // Reached attack range, stop walking
        animatorPlayer.SetInteger("AnimState", 0);  // Idle animation

        // Trigger attack
        if (Random.Range(1, 3) == 1)
        {
            Attack();
            isCrit = false;
        }
        else
        {
            Attack2();
            isCrit = true;
        }
    }

    public void Attack()
    {
        animatorPlayer.SetInteger("AnimState", 2);  // Attack animation
        StartCoroutine(ReturnToOriginalPosition());
    }
    public void Attack2()
    {
        animatorPlayer.SetInteger("AnimState", 6);  // Attack animation
        StartCoroutine(ReturnToOriginalPosition());
    }

    public IEnumerator ReturnToOriginalPosition()
    {
        yield return new WaitForSeconds(1); // Optional delay after attack
        if (isCrit)
        {
            battleHandler.UpdateDamageText(criticalDamage, true);
            enemyBehaviour.TakeDamage(criticalDamage);
        }
        else
        {
            battleHandler.UpdateDamageText(normalDamage, false);
            enemyBehaviour.TakeDamage(normalDamage);
        }
            
        animatorPlayer.SetInteger("AnimState", -1);  // Walk back to original position
        

        while (Vector3.Distance(transformPlayer.position, originalPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transformPlayer.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        animatorPlayer.SetInteger("AnimState", 0);  // Back to idle
        
        isMoving = false;
        isCrit = false;
    }

    public IEnumerator PlayerDamaged()
    {
        yield return new WaitForSeconds(0.6f);   // Walk back to original position
        animatorPlayer.SetInteger("AnimState", 0);

    }

    public IEnumerator delayAnimation(float enemyDamage)
    {
        yield return new WaitForSeconds(0.2f);   // Walk back to original position
        TakeDamage(enemyDamage, 0);
    }

    public IEnumerator PlayerDie(float second)
    {
        yield return new WaitForSeconds(second);
        Die();
    }

    public void TakeDamage(float enemyDamage, float second)
    {
        playerHealth -= enemyDamage;
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            StartCoroutine(PlayerDie(second));
        }
        else
        {
            animatorPlayer.SetInteger("AnimState", 5);  // Taking damage animation
            StartCoroutine(PlayerDamaged());
        }
        Debug.Log($"Player Health: {playerHealth}");
    }

    // Handling collisions with the enemy
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))  // Ensure it's the enemy
        {
            if (!isDetected)
            { 
            Debug.Log("Enemy collision detected");
            isDetected = true;
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isDetected = false;
            if (Random.Range(1, 3) == 1)
            {
                Attack();
                isCrit = false;
            }
            else
            {
                Attack2();
                isCrit = true;
            };
        }
    }

    public void Die()
    {
        animatorPlayer.SetInteger("AnimState", 4);  // Die animation
        StartCoroutine(waitToRevive());
        // Add logic for when character dies
    }

    IEnumerator waitToRevive()
    {
        yield return new WaitForSeconds(1.2f);
        characterDie.SetActive(true);
        characterPlayer.SetActive(false);
    }
}