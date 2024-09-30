using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    public Transform player;  // Player's transform
    public Transform enemy;
    public float moveSpeed = 1;
    public Animator animator;
    public float chaseRange = 5.0f; // Detection range for the player
    //public FollowPath followPath;   // Reference to FollowPath script for toggling
    private bool isChasingPlayer = false;

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange)
            {
                isChasingPlayer = true;
                //followPath.enabled = false; // Stop following waypoints when chasing player
                followTarget();  // Chase the player
            }
            else
            {
                isChasingPlayer = false;
               // followPath.enabled = true; // Return to waypoint movement
            }
        }
        else
        {
            animator.SetInteger("AnimState", 0);
        }
    }

    private void followTarget()
    {
        animator.SetInteger("AnimState",1);

        if (enemy.position.x < player.position.x)
        {
            // face right
            enemy.localScale = new Vector3(8, 6, 1);
        }
        else
        {
            // Keep facing left if moving left
            enemy.localScale = new Vector3(-8, 6, 1);
        }

        Vector3 distance = player.position - transform.position;
        transform.Translate(new Vector3(distance.x, 0, 0) * moveSpeed * Time.deltaTime);
    }
}
