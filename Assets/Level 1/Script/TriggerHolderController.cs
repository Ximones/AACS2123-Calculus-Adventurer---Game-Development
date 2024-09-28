using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHolderController : MonoBehaviour
{
    // This will be the child object that will disappear when the player exits the trigger.
    [SerializeField] private GameObject childObject;

    private bool playerInside = false; // Tracks if the player is inside the trigger

    void Start()
    {
        // Optional: Ensure the child object is visible at the start
        if (childObject != null)
        {
            childObject.SetActive(true);
        }
    }

    // Detect when the player enters the trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player entered the trigger area");

            // Optional: You can perform actions like showing something
            if (childObject != null)
            {
                childObject.SetActive(true);  // Make sure the child is visible
            }
        }
    }

    // Detect when the player exits the trigger collider
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("Player exited the trigger area");

            // Hide the child object when the player exits the trigger
            if (childObject != null)
            {
                childObject.SetActive(false);
            }
        }
    }

    // This method can be used if you need to check the player's status in the trigger
    public bool IsPlayerInside()
    {
        return playerInside;
    }
}