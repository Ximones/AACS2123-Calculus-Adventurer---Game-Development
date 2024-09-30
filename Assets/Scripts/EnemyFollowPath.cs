using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPath : MonoBehaviour
{
    public Transform waypoint1;  // First waypoint
    public Transform waypoint2;  // Second waypoint
    public Transform enemy;  // Enemy's transform
    public Transform player;  // Player's transform
    public Animator animator;
    public Vector3 originalposition;

    private int waypointIndex = 0;  // To keep track of the current waypoint (0 = waypoint1, 1 = waypoint2)
    public float moveSpeed = 1f;
    public float chaseSpeed = 2f;
    public float chaseRange = 5.0f;  // Detection range for the player
    private bool isChasingPlayer = false;

    void Start()
    {
        enemy.position = waypoint1.position;  // Start the enemy at the first waypoint
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(enemy.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            // Start chasing player if within chase range
            isChasingPlayer = true;
        }
        else
        {
            // Stop chasing player, return to path-following
            isChasingPlayer = false;
        }

        if (isChasingPlayer)
        {
            followTarget();
        }
        else
        {
            followPath();
        }
    }

    private void followPath()
    {
        animator.SetInteger("AnimState", 1);

        // Move the enemy towards the current waypoint
        Transform targetWaypoint = (waypointIndex == 0) ? waypoint1 : waypoint2;

        

        // Move enemy towards the current waypoint
        enemy.position = Vector3.MoveTowards(enemy.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        // Check if the enemy has reached the current waypoint
        float distanceToWaypoint = Vector3.Distance(enemy.position, targetWaypoint.position);
        if (distanceToWaypoint < 0.1f)
        {
            // Switch to the other waypoint
            waypointIndex = (waypointIndex == 0) ? 1 : 0;
        }

        // Flip the enemy to face the direction it's moving
        if (enemy.position.x < targetWaypoint.position.x)
        {
            // Face right
            enemy.localScale = new Vector3(Mathf.Abs(enemy.localScale.x), enemy.localScale.y, enemy.localScale.z);
            
        }
        else
        {
            // Face left
            enemy.localScale = new Vector3(-Mathf.Abs(enemy.localScale.x), enemy.localScale.y, enemy.localScale.z);
        }
    }

    private void followTarget()
    {
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
        enemy.position += new Vector3(direction.x, 0, 0) * chaseSpeed * Time.deltaTime;
    }
}
