using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBehaviour : Enemy
{ 
    // Start is called before the first frame update
    protected override void Init()
{

    enemyHealth = 100;
    moveSpeed = 5f;
    normalDamage = 20;
    criticalDamage = 40;
    Debug.Log("Goblin initialized");
}
public override void Move()
{
    Debug.Log("Goblin moves!");
    isMoving = true;
    StartCoroutine(WalkToPlayer());
}
private IEnumerator WalkToPlayer()
{
    Vector3 targetPosition = transformPlayer.position;

    // Determine which direction to face before starting to walk
    if (transformEnemy.position.x > targetPosition.x)
    {
        // Player is to the left, face left
        transformEnemy.localScale = new Vector3(-5, 3.6f, 1); // Face left (original scale)
    }
    else
    {
        // Player is to the right, face right
        transformEnemy.localScale = new Vector3(5, 3.6f, 1);  // Flip to face right
    }

    animatorEnemy.SetInteger("AnimState", 1);  // Walking animation

    // Walk towards the player
    while (Vector3.Distance(transformEnemy.position, targetPosition) > attackRange)
    {
        // Ensure scale remains correct every frame
        if (transformEnemy.position.x > targetPosition.x)
        {
            // Keep facing left if moving left
            transformEnemy.localScale = new Vector3(-5, 3.6f, 1);
        }
        else
        {
            // Keep facing right if moving right
            transformEnemy.localScale = new Vector3(5, 3.6f, 1);
        }

        // Move towards the target
        transformEnemy.position = Vector3.MoveTowards(transformEnemy.position, targetPosition, moveSpeed * Time.deltaTime);
        yield return null;  // Wait for the next frame
    }

    // After reaching the player, perform attack

    if (Random.Range(1, 3) == 1)
    {
        Attack();
    }
    else
    {
        Attack2();
    }
}

protected override void Attack()
{
    Debug.Log("Goblin attacks!");
    animatorEnemy.SetInteger("AnimState", 2);  // Goblin attack animation
                                               // Assume PlayerBehaviour is available and hooked up
    playerBehaviour.TakeDamage(normalDamage, 1);
    battleHandler.UpdateDamageText(normalDamage,false);
    StartCoroutine(ReturnToOriginalPosition());
}

protected override void Attack2()
{
    Debug.Log("Goblin attacks!");
    animatorEnemy.SetInteger("AnimState", 6);  // Goblin attack animation
                                               // Assume PlayerBehaviour is available and hooked up
    playerBehaviour.TakeDamage(criticalDamage, 1);
    battleHandler.UpdateDamageText(criticalDamage, true);
    StartCoroutine(ReturnToOriginalPosition());
}



    private IEnumerator ReturnToOriginalPosition()
    {
        yield return new WaitForSeconds(0.5f);  // Optional delay after attack

        // Determine which direction to face before walking back
        if (transformEnemy.position.x < originalPosition.x)
        {
            // Move to the right, so face right
            transformEnemy.localScale = new Vector3(5, 4, 1);  // Face right
        }

        animatorEnemy.SetInteger("AnimState", 1);  // Walking animation

        // Save the original Y position to keep it constant during movement
        float originalY = transformEnemy.position.y;

        // Walk back to the original position
        while (Vector3.Distance(transformEnemy.position, originalPosition) > 1f)
        {
            // Ensure the enemy is facing the right direction
            if (transformEnemy.position.x < originalPosition.x)
            {
                // Keep facing right if moving right
                transformEnemy.localScale = new Vector3(5, 4, 1);
            }
            else
            {
                // Keep facing left if moving left
                transformEnemy.localScale = new Vector3(-5, 4, 1);
            }

            // Move only along the X-axis, keep Y-axis and Z-axis constant
            transformEnemy.position = new Vector3(
                Mathf.MoveTowards(transformEnemy.position.x, originalPosition.x, moveSpeed * Time.deltaTime),
                originalY,  // Keep Y constant
                transformEnemy.position.z // Keep Z constant (in case Z is used)
            );

            yield return null;  // Wait for the next frame
        }

        // After reaching the original position, switch to idle state
        animatorEnemy.SetInteger("AnimState", 0);  // Idle animation

        // Ensure the enemy is facing left (original direction)
        transformEnemy.localScale = new Vector3(-5, 4, 1);  // Reset to face left

        // Set the enemy as no longer moving
        isMoving = false;
    }
}
