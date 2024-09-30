using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Array to store the game objects in Level 1
    public GameObject[] gameObjectsInLevel1;
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

        // Load all the game objects from scene to save
        gameObjectsInLevel1 = UnityEngine.Object.FindObjectsOfType<GameObject>();

    }

    // Call this method when transitioning to the combat scene
    public void EnterCombatScene(string combatSceneName)
    {

        // Save the current state of Level 1 before leaving
        saveManager.SaveSceneState(gameObjectsInLevel1);
        // Load the combat scene
        SceneManager.LoadScene(combatSceneName);
    }

    // Call this method when returning from the combat scene
    public void ReturnToLevel1()
    {
        // Load Level 1 scene
        SceneManager.LoadScene(mapSceneName);
  
        StartCoroutine(WaitForLevel1Load());
    }


   private IEnumerator WaitForLevel1Load()
   {
       // Wait until the Level 1 scene is fully loaded
       yield return new WaitUntil(() => SceneManager.GetActiveScene().name == mapSceneName);
        saveManager.LoadSceneState();

    }

    // Mark an enemy for destruction
    public void MarkEnemyForDestruction(GameObject enemy)
    {
        enemy.SetActive(false);  // Deactivate the enemy so it won't be restored
    }
}
