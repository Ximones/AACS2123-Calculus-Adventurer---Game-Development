using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueScript; // Reference to the Dialogue script
    private bool playerDetected; // Flag to check if the player is detected

    // Detect trigger with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If we triggered the player, enable playerDetected and show the indicator
        if (collision.tag == "Player")
        {
            playerDetected = true;
            dialogueScript.ToggleIndicator(playerDetected);
        }
    }

    // Detect when the player exits the trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        // If we lost trigger with the player, disable playerDetected and hide the indicator
        if (collision.tag == "Player")
        {
            playerDetected = false;
            dialogueScript.ToggleIndicator(playerDetected);
            dialogueScript.EndDialogue(); // End the dialogue when the player leaves
        }
    }

    // While detected, if we interact, start the dialogue
    private void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.E))
        {
            dialogueScript.StartDialogue();
        }
    }
}