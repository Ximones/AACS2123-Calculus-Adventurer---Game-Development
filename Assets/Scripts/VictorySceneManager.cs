using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictorySceneManager : MonoBehaviour
{
    public GameObject victoryCanvas; 
    public Button nextLevelButton;
    public Button backToMenuButton;
    public AudioSource buttonClickAudioSource;
    public string playerTag = "Player";

    public PlayerController PlayerController;

    void Start()
    {
        victoryCanvas.SetActive(false); 
        nextLevelButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(NextLevel)));
        backToMenuButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(BackToMenu)));
    }

    void OnTriggerEnter2D(Collider2D other) // Player hit portal colider then set victory screen as true
    {
        if (other.CompareTag(playerTag))  // Check for player collision
        {
            if (PlayerController != null)
            {
                PlayerController.enabled = false; // Disable player movement
            }
            ShowVictoryScreen();
        }
    }

    public void ShowVictoryScreen()
    {
        victoryCanvas.SetActive(true);

        if (PlayerController != null)
        {
            PlayerController.enabled = false;
        }
    }

    private IEnumerator HandleButtonClick(System.Action action)
    {
        PlayButtonClickSound();
        yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds
        action.Invoke();
    }

    private void NextLevel()
    {
        if (PlayerController != null)
        {
            PlayerController.enabled = true;
        }

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels to load.");
        }
    }

    private void BackToMenu()
    {
        if (PlayerController != null)
        {
            PlayerController.enabled = true;
        }

        SceneManager.LoadScene("Main Menu"); // Load the main menu scene
    }

    private void PlayButtonClickSound()
    {
        if (buttonClickAudioSource != null && buttonClickAudioSource.clip != null)
        {
            buttonClickAudioSource.Play();
        }
    }
}
