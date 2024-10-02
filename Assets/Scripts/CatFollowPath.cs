using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class CatFollowPath : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform cat;
    public Transform player;
    public Animator animator;

    public GameObject lost;
    public GameObject catLost;
    public float moveSpeed = 1f;
    public float chaseSpeed = 2f;
    public float chaseRange = 5.0f;
    private int waypointIndex = 0;
    private bool isIdle = false;
    private bool isChasingPlayer = false;
    private bool isMoving = true;

    public AudioSource backgroundMusic;  // Reference to the AudioSource for background music
    public AudioClip newBackgroundMusic; // The new music to switch to after 20 seconds

    public AudioSource MeowSound;
    public AudioClip Meow;

    private Coroutine currentCoroutine;  // Track the currently running coroutine
    private Coroutine meowCoroutine;  // To keep track of the meowing coroutine

    private float timeInCollider = 0f;  // To track how long the player stays in the collider
    private bool isMusicChanged = false; // To track if the music has been changed

    void Start()
    {
        lost.SetActive(true);
        catLost.SetActive(false);
        MeowSound = gameObject.AddComponent<AudioSource>();
        cat.position = waypoints[0].position;
        currentCoroutine = StartCoroutine(FollowPath());
    }

    // Coroutine to follow waypoints
    IEnumerator FollowPath()
    {
        while (!isChasingPlayer && isMoving)
        {
            animator.SetInteger("AnimState", 1); // Walking animation

            Transform targetWaypoint = waypoints[waypointIndex].transform;

            cat.position = Vector3.MoveTowards(cat.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(cat.position, targetWaypoint.position) < 0.1f)
            {
                int animState = Random.Range(3, 8);
                StartCoroutine(PerformIdleAction(animState));

                waypointIndex++;
                if (waypointIndex == waypoints.Length)
                {
                    waypointIndex = 0;
                }
            }

            // Flip the cat to face the direction of movement
            if (cat.position.x < targetWaypoint.position.x)
                cat.localScale = new Vector3(Mathf.Abs(cat.localScale.x), cat.localScale.y, cat.localScale.z);
            else
                cat.localScale = new Vector3(-Mathf.Abs(cat.localScale.x), cat.localScale.y, cat.localScale.z);

            yield return null;  // Continue the coroutine
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasingPlayer = true;
            isMoving = false;  // Stop following the path
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);  // Stop FollowPath coroutine
            currentCoroutine = StartCoroutine(ChasePlayer());  // Start chasing player

            MeowSound.PlayOneShot(Meow);
            // Start the coroutine to meow every 2 seconds
            if (meowCoroutine == null)
            {
                meowCoroutine = StartCoroutine(MeowEvery2Seconds());
            }
        }
    }

    void Update()
    {
        if (isChasingPlayer)
        {
            timeInCollider += Time.deltaTime;

            // If player has been in the collider for more than 10 seconds and music hasn't changed yet
            if (timeInCollider >= 10f && !isMusicChanged)
            {
                ChangeBackgroundMusic();
                isMusicChanged = true;  // Prevents multiple music changes
                lost.SetActive(false);
                catLost.SetActive(true);
            }
        }
    }

    // Coroutine to meow every 2 seconds
    IEnumerator MeowEvery2Seconds()
    {
        yield return new WaitForSeconds(2f);  // Initial delay of 2 seconds

        while (true)  // Repeat the meow every 2 seconds
        {
            MeowSound.PlayOneShot(Meow);  // Play the meow sound
            yield return new WaitForSeconds(2f);  // Wait for 2 seconds before meowing again
        }
    }

    // Function to change the background music
    void ChangeBackgroundMusic()
    {
        backgroundMusic.clip = newBackgroundMusic;  // Set the new music clip
        backgroundMusic.Play();  // Play the new music
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isChasingPlayer = false;
            isMoving = true;  // Resume following the path
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);  // Stop chasing player
            currentCoroutine = StartCoroutine(FollowPath());  // Resume following path

            // Stop the meowing coroutine if the player exits the trigger
            if (meowCoroutine != null)
            {
                StopCoroutine(meowCoroutine);
                meowCoroutine = null;  // Reset to prevent re-start issues
            }

            // Reset time in collider and music change state
            timeInCollider = 0f;
            isMusicChanged = false;
        }
        MeowSound.Stop();
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

        animator.SetInteger("AnimState", 0);
        // Resume moving
        isIdle = false;
        isMoving = true;

        if (!isChasingPlayer)  // Only restart FollowPath if not chasing player
        {
            currentCoroutine = StartCoroutine(FollowPath());
        }
    }
}
