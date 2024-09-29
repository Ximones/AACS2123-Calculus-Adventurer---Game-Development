using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyCombatTrigger : MonoBehaviour
{
    public string combatSceneName = "Forest";  // Combat scene name
    public PlayerController playerController;  // Reference to the player controller
    private Vector3 playerOriginalPosition;    // Store the player's original position

    private void Start()
    {
        // Store the player's position at the start
        playerOriginalPosition = playerController.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerController != null)
            {
                playerController.enabled = false;  // Disable player movement
            }

            // Set the current enemy in the GameManager
            GameManager.SetEnemyObject(this.gameObject);  // Store reference to this enemy object
            TriggerCombat();
        }
    }

    private void TriggerCombat()
    {
        Debug.Log("Player has collided with the enemy! Loading combat scene...");

        // Save player position before switching to combat scene
        GameManager.Instance.SavePlayerPosition(playerOriginalPosition);
        SceneManager.LoadScene(combatSceneName);  // Load the combat scene
    }
}
