using System.Collections;
using UnityEngine;

public class NightBringerBehaviour : Enemy
{
    [SerializeField] private GameObject spellEnemy;
    [SerializeField] private Animator animatorSpell;

    protected override void Init()
    {
        
        // Initialize spellEnemy
        if (spellEnemy == null)
        {
            spellEnemy = GameObject.FindGameObjectWithTag("Spell");
        }
        else
        {
            Debug.Log("Found Enemy Spell");
        }

        // Initialize spell animator
        if (animatorSpell == null)
        {
            animatorSpell = spellEnemy.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("Found Spell Animator");
        }

        // Set enemy-specific stats
        enemyHealth = 100;
        moveSpeed = 4f;
        normalDamage = 50;
        criticalDamage = 50;
        Debug.Log("NightBringer initialized");
    }


    public override void Move()
    {
        
        isMoving = true;
        StartCoroutine(WalkToPlayer());
    }

    private IEnumerator WalkToPlayer()
    {
        // Decide between two types of attacks randomly
        if (Random.Range(1, 3) == 1)
        {
            originalPosition = characterEnemy.transform.position;
            Vector3 targetPosition = transformPlayer.position;

            // Start walking animation
            animatorEnemy.SetInteger("AnimState", 1);  // Walk animation

            // Move toward the player until within attack range
            while (Vector3.Distance(transformEnemy.position, targetPosition) > attackRange)
            {
                transformEnemy.position = Vector3.MoveTowards(transformEnemy.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;  // Wait for the next frame
            }

            // Perform regular attack
            Attack();
        }
        else
        {
            // Perform special spell attack
            Attack2();
            yield return null;
        }
    }

    protected override void Attack()
    {
        
        // Regular attack animation and behavior
        animatorEnemy.SetInteger("AnimState", 2);  // Attack animation
        playerBehaviour.TakeDamage(normalDamage, 0.2f);  // Player takes damage

        // Return to original position after attacking
        StartCoroutine(ReturnToOriginalPosition());
    }

    protected override void Attack2()
    {
        // Special attack with spell
        animatorEnemy.SetInteger("AnimState", 6);  // Special attack animation
        spellEnemy.SetActive(true);  // Activate spell
        animatorSpell.SetInteger("AnimState", 1);  // Spell cast animation

        // Start coroutine to deactivate the spell after use
        StartCoroutine(DeactivateSpell());
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        yield return new WaitForSeconds(1f);  // Optional delay after attack

        // Walk back to the original position
        animatorEnemy.SetInteger("AnimState", 1);  // Walk back animation

        while (Vector3.Distance(transformEnemy.position, originalPosition) > 0.1f)
        {
            transformEnemy.position = Vector3.MoveTowards(transformEnemy.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        animatorEnemy.SetInteger("AnimState", 0);  // Back to idle
        isMoving = false;
    }

    private IEnumerator DeactivateSpell()
    {
        yield return new WaitForSeconds(1f);  // Wait for spell to finish
        spellEnemy.SetActive(false);  // Deactivate the spell

        // Apply spell damage to the player
        playerBehaviour.TakeDamage(criticalDamage, 0);

        // Return to idle
        StartCoroutine(ReturnToIdle());
    }

    private IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(1f);  // Optional delay
        animatorEnemy.SetInteger("AnimState", 0);  // Back to idle
        isMoving = false;
    }
}
