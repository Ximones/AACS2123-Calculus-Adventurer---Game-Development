using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Color TeamColor; // new variable declared

    public static GameObject currentEnemy;  // Static reference to the enemy that initiated combat
    private Vector3 savedPlayerPosition;  // Store player’s position before combat
   
    private void Awake()
    {
        // start of new code
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
   
    public static void SetEnemyObject(GameObject objectEnemy)
    {
        currentEnemy = objectEnemy;
    }

    public void SavePlayerPosition(Vector3 position)
    {
        savedPlayerPosition = position;
    }

    public void ReturnToPreviousScene()
    {
        SceneManager.LoadScene("Level 1");  // Replace with your overworld scene name
        StartCoroutine(WaitForSceneLoad());  // Start coroutine to wait for scene load and restore player state
    }

    private IEnumerator WaitForSceneLoad()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Level 1");  // Replace with the correct overworld scene name

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.transform.position = savedPlayerPosition;  // Restore player’s position
            player.enabled = true;  // Enable player movement
        }

        // Destroy the enemy that triggered combat
        if (currentEnemy != null)
        {
            Destroy(currentEnemy);  // Remove the enemy object from the scene
            currentEnemy = null;  // Clear reference after destroying the enemy
        }
    }
}
