using System.Collections;
using UnityEngine;

public class NightBringerBehaviour : Enemy
{
    [SerializeField] private GameObject spellEnemy;
    [SerializeField] private Animator animatorSpell;

    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip specialAttackClip;
    [SerializeField] private AudioClip spellClip;

    private AudioSource audioSource;

    protected override void Init()
    {
        // Initialize AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

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
        enemyName = "Night Bringer";
        enemyHealth = 100;
        moveSpeed = 4f;
        normalDamage = 50;
        criticalDamage = 100;
        Debug.Log("NightBringer initialized");
    }

    public override void Move()
    {
        isMoving = true;
        PlaySound(walkClip, true);  // Play walking sound in loop
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

            // Stop walking sound
            audioSource.loop = false;
            audioSource.Stop();

            // Perform regular attack
            Attack();
        }
        else
        {
            // Stop walking sound
            audioSource.loop = false;
            audioSource.Stop();

            // Perform special spell attack
            Attack2();
            yield return null;
        }
    }

    protected override void Attack()
    {
        // Regular attack animation and behavior
        animatorEnemy.SetInteger("AnimState", 2);  // Attack animation
        PlaySound(attackClip);  // Play attack sound
        playerBehaviour.TakeDamage(normalDamage, 0.2f);  // Player takes damage
        battleHandler.UpdateDamageText(normalDamage, false);
        // Return to original position after attacking
        StartCoroutine(ReturnToOriginalPosition());
    }

    protected override void Attack2()
    {
        // Special attack with spell
        animatorEnemy.SetInteger("AnimState", 6);  // Special attack animation
        PlaySound(specialAttackClip);  // Play special attack sound
        spellEnemy.SetActive(true);  // Activate spell
        animatorSpell.SetInteger("AnimState", 1);  // Spell cast animation
        PlaySound(spellClip);  // Play spell sound

        // Start coroutine to deactivate the spell after use
        StartCoroutine(DeactivateSpell());
    }

    private IEnumerator ReturnToOriginalPosition()
    {
        yield return new WaitForSeconds(1f);  // Optional delay after attack

        // Walk back to the original position
        animatorEnemy.SetInteger("AnimState", 1);  // Walk back animation
        PlaySound(walkClip, true);  // Play walking sound in loop

        while (Vector3.Distance(transformEnemy.position, originalPosition) > 0.1f)
        {
            transformEnemy.position = Vector3.MoveTowards(transformEnemy.position, originalPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Stop walking sound
        audioSource.loop = false;
        audioSource.Stop();

        animatorEnemy.SetInteger("AnimState", 0);  // Back to idle
        isMoving = false;
    }

    private IEnumerator DeactivateSpell()
    {
        yield return new WaitForSeconds(1f);  // Wait for spell to finish

        spellEnemy.SetActive(false);  // Deactivate the spell

        // Apply spell damage to the player
        playerBehaviour.TakeDamage(criticalDamage, 0);
        battleHandler.UpdateDamageText(criticalDamage, true);

        // Return to idle
        StartCoroutine(ReturnToIdle());
    }

    private IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(1f);  // Optional delay
        animatorEnemy.SetInteger("AnimState", 0);  // Back to idle
        isMoving = false;
    }

    private void PlaySound(AudioClip clip, bool loop = false)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }
}