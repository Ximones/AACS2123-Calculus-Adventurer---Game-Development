using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    // Reference to the object you want to activate/deactivate
    public GameObject targetObject;

    // Method called when another collider enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player entered the trigger zone
        if (other.CompareTag("Player"))
        {
            // Enable the target object (or set active)
            targetObject.SetActive(true);
            Debug.Log("Player entered the trigger, object activated.");
        }
    }

    // Method called when another collider exits the trigger collider
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the player exited the trigger zone
        if (other.CompareTag("Player"))
        {
            // Disable the target object (or set inactive)
            targetObject.SetActive(false);
            Debug.Log("Player exited the trigger, object deactivated.");
        }
    }
}
