using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance;

    public List<GameObject> gameObjectsInLevel = new List<GameObject>();
    public string mapSceneName = "";  // Scene to quit for combat
    public SaveManager saveManager;      // Reference to SaveManager
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep GameManager alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (saveManager == null)
        {
            saveManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SaveManager>();
        }

        foreach (var obj in FindObjectsOfType<GameObject>())
        {
            if (obj.activeInHierarchy) // Only add active objects
            {
                gameObjectsInLevel.Add(obj);
            }
        }

    }

    // Call this method when transitioning to the combat scene
    public void ReturnToLevel()
    {
        // Load Level 1 scene
        SceneManager.LoadScene(mapSceneName);
        StartCoroutine(WaitForLevel1Load());
    }
    public void EnterCombatScene(string combatSceneName)
    {

        // Save the current state of Level 1 before leaving
        saveManager.SaveSceneState(gameObjectsInLevel);
        // Load the combat scene
        SceneManager.LoadScene(combatSceneName);
    }

    // Call this method when returning from the combat scene
    private IEnumerator WaitForLevel1Load()
    {
        // Wait until the Level 1 scene is fully loaded
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == mapSceneName);
        saveManager.LoadSceneState();
    }

    // Method to mark an enemy for destruction
    public void MarkEnemyForDestruction(GameObject enemy)
    {
        enemy.SetActive(false);  // Deactivate the enemy so it won't be restored
    }

    public void TogglePause()
    {
        Time.timeScale = 0.0f; // Pause the game
    }

    public void StopPause()
    {

        Time.timeScale = 1.0f; // Resume normal time
    }
}
