using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 8f;
    public bool isWalking = false;
    public bool isGrounded = true;
    public bool isLanding = true;
    public bool canMove = true;
    public bool canDash = true;

    public bool isJumpCooldown = false;

    public bool isDashCooldown = false;

    public float jumpForce = 7f;
    public float jumpDelay = 0.4f;
    public float jumpCooldown = 0.5f;

    public float dashForce = 10f;
    public float dashCooldown = 1f;
    private float lastDashTime = 0f;

    public int dashCount = 0;
    public int jumpCount = 0;

    private Animator playerAnimator;
    private Rigidbody2D playerBody;

    // AudioSource component for playing sound effects
    private AudioSource audioSource;

    // AudioClip fields for sound effects
    public AudioClip jumpClip;
    public AudioClip doubleJumpClip;
    public AudioClip dashClip;
    public AudioClip landingClip;
    public AudioClip walkClip;

    // Variable to track the previous walking state
    private bool wasWalking = false;

    void Start()
    {
        canDash = true;
        if (playerBody == null)
            playerBody = GetComponent<Rigidbody2D>();
        else
            Debug.Log("Player Rigid Body 2D not found");

        if (playerAnimator == null)
            playerAnimator = GetComponent<Animator>();
        else
            Debug.Log("Player Animator not found");

        // Ensure AudioSource component is attached
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ensure AudioClip fields are assigned
        if (jumpClip == null || doubleJumpClip == null || dashClip == null || landingClip == null || walkClip == null)
            Debug.LogError("One or more AudioClip fields are not assigned.");
    }

    void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleDash();

    }

    private void HandleMovement()
    {
        float playerHorizontalInput = Input.GetAxis("Horizontal");
        playerAnimator.SetBool("CanMove", canMove);
        playerAnimator.SetFloat("AnimState", playerHorizontalInput);
        playerBody.velocity = new Vector2(playerHorizontalInput * playerSpeed, playerBody.velocity.y);

        isWalking = playerHorizontalInput != 0;
        playerAnimator.SetBool("isWalking", isWalking);

        if (isWalking && !wasWalking && isGrounded)
        {
            PlayWalkingSound();
        }
        else if (!isWalking && wasWalking)
        {
            StopWalkingSound();
        }

        wasWalking = isWalking;
    }

    private void PlayWalkingSound()
    {
        audioSource.clip = walkClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void StopWalkingSound()
    {
        audioSource.Stop();
    }

    private void HandleJumping()
    {
        if (isJumpCooldown == true)
        {
            StartCoroutine(JumpCooldown());
            return;
        }

        canMove = checkValidJumpCount(jumpCount);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canMove)
        {
            jumpCount++;
            playerAnimator.SetBool("CanMove", canMove);
            StartCoroutine(PerformJump());
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && canMove)
        {
            jumpCount++;
            playerAnimator.SetBool("CanMove", canMove);
            StartCoroutine(PerformJump());
        }
    }

    private void HandleDash()
    {


        // If dash cooldown is active or player has already dashed twice, don't allow dashing
        if (isDashCooldown || dashCount > 1)
        {
            return;
        }

        // Check if player presses dash key and is currently walking
        if (Input.GetKeyDown(KeyCode.C) && isWalking)
        {
            dashCount++;  // Increase dash count
            float playerHorizontalInput = Input.GetAxis("Horizontal");
            StartCoroutine(PerformDash(playerHorizontalInput));  // Start dash coroutine
        }
    }

    IEnumerator PerformJump()
    {
        if (jumpCount == 1)
        {
            playerAnimator.SetTrigger("isJumpStart");
            yield return new WaitForSeconds(0.1f);
            playerBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            audioSource.PlayOneShot(jumpClip); // Play jump sound effect
            performLanding();
        }

        if (jumpCount == 2)
        {
            playerBody.velocity = new Vector2(playerBody.velocity.x, 0);
            playerAnimator.SetTrigger("isDoubleJump");
            yield return new WaitForSeconds(0.1f);
            playerBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            audioSource.PlayOneShot(doubleJumpClip); // Play double jump sound effect
            performLanding();
            canMove = checkValidJumpCount(jumpCount);
            playerAnimator.SetBool("CanMove", canMove);
        }
    }

    IEnumerator PerformDash(float directionInput)
    {

        // Set animator and flags for dashing
        canDash = false;
        playerAnimator.SetBool("CanDash", true);

        // Only dash if cooldown is not active and conditions are met
        if (dashCount == 1 && Time.time >= lastDashTime + dashCooldown && isWalking)
        {
            playerAnimator.SetTrigger("isDashStart");

            // Calculate dash direction and apply force
            float dashDirectionX = directionInput < 0 ? -1 : 1;
            playerBody.AddForce(new Vector2(dashDirectionX * dashForce, 0), ForceMode2D.Impulse);
            audioSource.PlayOneShot(dashClip); // Play dash sound effect

            // Set cooldown flags and timing
            isDashCooldown = true;
            lastDashTime = Time.time;

            // Wait for dash duration before allowing cooldown
            yield return new WaitForSeconds(0.5f);

            // Start cooldown coroutine
            StartCoroutine(DashCooldown());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            HandleGroundCollision();
        }
    }

    private void HandleGroundCollision()
    {
        performLanded();
        jumpCount = 0;
        canMove = checkValidJumpCount(jumpCount);
        playerAnimator.SetBool("CanMove", canMove);
        playerAnimator.ResetTrigger("isJumpStart");
        playerAnimator.ResetTrigger("isDoubleJump");
        isJumpCooldown = true;
        audioSource.PlayOneShot(landingClip); // Play landing sound effect
    }

    private void performLanding()
    {
        isGrounded = false;
        isLanding = true;
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetBool("isLanding", isLanding);
    }

    private void performLanded()
    {
        isGrounded = true;
        isLanding = false;
        playerAnimator.SetBool("isGrounded", true);
        playerAnimator.SetBool("isLanding", false);
    }

    IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(jumpCooldown);
        isJumpCooldown = false;
    }

    IEnumerator DashCooldown()
    {
        playerAnimator.SetBool("CanDash", canDash);
        yield return new WaitForSeconds(dashCooldown);

        // Reset dash count and cooldown state
        dashCount = 0;
        isDashCooldown = false;
        canDash = true;  // Re-enable dashing after cooldown
        
    }

    private bool checkValidJumpCount(int jumpCount)
    {
        return jumpCount < 2;
    }
}