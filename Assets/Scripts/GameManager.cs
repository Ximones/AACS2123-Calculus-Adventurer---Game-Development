using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // List of objects to save from Level 1
    public GameObject[] gameObjectsInLevel1;
    public string sceneName = "Forest";
    // Reference to the SaveManager
    private SaveManager saveManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        saveManager = GetComponent<SaveManager>();

        // Check if we are returning to "Level 1" and restore saved state
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            saveManager.LoadSceneState();  // Restore saved game objects
        }
    }

    // Call this when transitioning to the combat scene
    public void EnterCombatScene()
    {
        // Save the current state of Level 1 before leaving
        saveManager.SaveSceneState(gameObjectsInLevel1);

        // Load the combat scene
        SceneManager.LoadScene(sceneName);
    }

    // Call this when returning from the combat scene
    public void ReturnToLevel1()
    {
        // Load Level 1 scene
        SceneManager.LoadScene("Level 1");

        // Once the scene is loaded, restore the saved state
        StartCoroutine(WaitForLevel1Load());

    }

    private IEnumerator WaitForLevel1Load()
    {
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Level 1");

        // Restore the saved GameObject states after the scene is loaded
        saveManager.LoadSceneState();
    }

    // Mark an enemy for destruction
    public void MarkEnemyForDestruction(GameObject enemy)
    {
       Destroy(enemy);  // Deactivate the enemy, so it won't be restored
    }
}
