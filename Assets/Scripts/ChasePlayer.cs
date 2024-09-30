using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    public Transform player;  // Player's transform
    public Transform enemy;
    public float moveSpeed = 1;
    public Animator animator;
    public float chaseRange = 5.0f;  // Detection range for the player
    public EnemyFollowPath followPath;  // Reference to the path-following script
    private bool isChasingPlayer = false;

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(enemy.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            // Start chasing player
            isChasingPlayer = true;
            followPath.enabled = false;  // Disable path-following when chasing
            followTarget();  // Chase the player
        }
        else
        {
            // Stop chasing player, return to path-following
            isChasingPlayer = false;
            followPath.enabled = true;  // Re-enable path-following
        }
    }

    private void followTarget()
    {
        // Set walking animation
        animator.SetInteger("AnimState", 1);

        // Flip the enemy to face the player based on x position
        if (enemy.position.x < player.position.x)
        {
            // Face right
            enemy.localScale = new Vector3(Mathf.Abs(enemy.localScale.x), enemy.localScale.y, enemy.localScale.z);
        }
        else
        {
            // Face left
            enemy.localScale = new Vector3(-Mathf.Abs(enemy.localScale.x), enemy.localScale.y, enemy.localScale.z);
        }

        // Move towards the player
        Vector3 direction = (player.position - enemy.position).normalized;
        enemy.position += new Vector3(direction.x, 0, 0) * moveSpeed * Time.deltaTime;
    }
}
