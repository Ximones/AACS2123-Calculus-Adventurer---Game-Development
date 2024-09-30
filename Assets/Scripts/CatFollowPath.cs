using System.Collections;
using UnityEngine;

public class CatFollowPath : MonoBehaviour
{
    public Transform waypoint1;
    public Transform waypoint2;
    public Transform cat;
    public Transform player;
    public Animator animator;

    public float moveSpeed = 1f;
    public float chaseSpeed = 2f;
    public float chaseRange = 5.0f;
    private int waypointIndex = 0;
    private bool isIdle = false;
    private bool isChasingPlayer = false;
    private bool isMoving = true;

    void Start()
    {
        cat.position = waypoint1.position;
        StartCoroutine(FollowPath());
    }

    // Coroutine to follow waypoints
    IEnumerator FollowPath()
    {
        while (true)
        {
            if (!isChasingPlayer && isMoving)
            {
                animator.SetInteger("AnimState", 1); // Walking animation

                Transform targetWaypoint = (waypointIndex == 0) ? waypoint1 : waypoint2;
                cat.position = Vector3.MoveTowards(cat.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(cat.position, targetWaypoint.position) < 0.1f)
                {
                    waypointIndex = (waypointIndex == 0) ? 1 : 0;

                    int animState = Random.Range(4, 8);

                    StartCoroutine(PerformIdleAction(animState));

                }

                // Flip the cat to face the direction of movement
                if (cat.position.x < targetWaypoint.position.x)
                    cat.localScale = new Vector3(Mathf.Abs(cat.localScale.x), cat.localScale.y, cat.localScale.z);
                else
                    cat.localScale = new Vector3(-Mathf.Abs(cat.localScale.x), cat.localScale.y, cat.localScale.z);
            }

            yield return null; // Continue the coroutine
        }
    }

    // Detect when the player enters the chase range
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasingPlayer = true;
            isMoving = false;  // Stop following the path
            StartCoroutine(ChasePlayer());
        }
    }

    // Detect when the player leaves the chase range
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasingPlayer = false;
            isMoving = true;  // Resume following the path
            StopCoroutine(ChasePlayer());
            StartCoroutine(FollowPath());  // Resume following path
        }
    }

    // Coroutine to chase the player
    IEnumerator ChasePlayer()
    {
        while (isChasingPlayer)
        {
            animator.SetInteger("AnimState", 2); // Running animation

            if (cat.position.x < player.position.x)
                cat.localScale = new Vector3(Mathf.Abs(cat.localScale.x), cat.localScale.y, cat.localScale.z);
            else
                cat.localScale = new Vector3(-Mathf.Abs(cat.localScale.x), cat.localScale.y, cat.localScale.z);

            Vector3 direction = (player.position - cat.position).normalized;
            cat.position += new Vector3(direction.x, 0, 0) * chaseSpeed * Time.deltaTime;

            yield return null;  // Continue chasing until player leaves the range
        }
    }

    // Example coroutines for random idle actions when the player is not detected
    IEnumerator PerformIdleAction(int animState)
    {
        isIdle = true;
        isMoving = false;  // Pause path following during idle action

        // Set the idle animation state
        animator.SetInteger("AnimState", animState);

        // Wait for the idle animation to finish
        yield return new WaitForSeconds(1);

        // Resume moving
        isIdle = false;
        isMoving = true;
        StartCoroutine(FollowPath());
    }

    // Trigger idle animations when cat is not chasing or following the player
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!isChasingPlayer && !isIdle)
            {
                int animState = Random.Range(4, 8);  // Select random idle action
                StartCoroutine(PerformIdleAction(animState));
            }
        }
    }
}
