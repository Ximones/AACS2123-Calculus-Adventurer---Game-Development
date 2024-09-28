using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Animator animator;  
    [SerializeField] private AudioSource audioSource; 

    [SerializeField] private AudioClip openClip;  
    [SerializeField] private AudioClip closeClip; 

    [SerializeField] private KeyCode interactKey = KeyCode.E; // Key to interact 

    private bool isOpen = false;  // Track is currently open or closed
    private bool playerIsNear = false; // Track whether the player is nearby

    private void Update()
    {
        // Check if the player is near and presses the interact key
        if (playerIsNear && Input.GetKeyDown(interactKey))
        {
            toggle(); // Toggle between opening and closing
        }
    }

    private void toggle()
    {
        if (isOpen)
        {
            Close();
        }
        else
        {
            open();
        }
    }

    private void open()
    {
        // Play the open chest animation
        animator.SetTrigger("Open");

        // Play the open chest sound effect
        if (audioSource != null && openClip != null)
        {
            audioSource.clip = openClip;
            audioSource.Play();
        }

        isOpen = true; // Set chest state to open
    }

    private void Close()
    {
        // Play the close chest animation
        animator.SetTrigger("Close");

        // Play the close chest sound effect
        if (audioSource != null && closeClip != null)
        {
            audioSource.clip = closeClip;
            audioSource.Play();
        }

        isOpen = false; // Set chest state to closed
    }

    // Unity's built-in function that gets called when a Collider2D enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player has entered the chest's collider
        if (other.CompareTag("Player"))
        {
            playerIsNear = true; // Player is now within interaction range
        }
    }

    // Unity's built-in function that gets called when a Collider2D exits the trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player has exited the chest's collider
        if (other.CompareTag("Player"))
        {
            playerIsNear = false; // Player is no longer within interaction range
        }
    }
}

