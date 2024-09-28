using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathSceneManager : MonoBehaviour
{
    public GameObject deathSceneCanvas; // Reference to the death scene canvas
    public Button reviveButton;
    public Button restartButton;
    public Button quitButton;
    public AudioSource buttonClickSound;

    void Start()
    {
        Debug.Log("DeathSceneManager Start method called");
        deathSceneCanvas.SetActive(false); // Ensure the death scene canvas is hidden at the start
        reviveButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        reviveButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(Revive)));
        restartButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(Restart)));
        quitButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(Quit)));
    }

    public void ShowDeathScene(bool showReviveButton)
    {
        Debug.Log("ShowDeathScene method called");
        deathSceneCanvas.SetActive(true);
        reviveButton.gameObject.SetActive(showReviveButton);
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        DisablePlayerMovement();
    }

    private IEnumerator HandleButtonClick(System.Action action)
    {
        PlayButtonClickSound();
        yield return new WaitForSeconds(2); // Wait for 2 seconds
        action.Invoke();
    }

    private void Revive()
    {
        Debug.Log("Revive method called");
        deathSceneCanvas.SetActive(false);
        EnablePlayerMovement();
    }

    private void Restart()
    {
        Debug.Log("Restart method called");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    private void Quit()
    {
        Debug.Log("Quit method called");
        SceneManager.LoadScene("Main Menu");
    }

    private void PlayButtonClickSound()
    {
        if (buttonClickSound != null && buttonClickSound.clip != null)
        {
            buttonClickSound.Play();
        }
    }

    private void DisablePlayerMovement()
    {
        Debug.Log("DisablePlayerMovement method called");
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerController>().enabled = false;
        }
    }

    private void EnablePlayerMovement()
    {
        Debug.Log("EnablePlayerMovement method called");
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D method called");
        Debug.Log("Collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Death"))
        {
            Debug.Log("Collision with Death object detected");

            // Get the player component
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    Debug.Log("Player component found");
                    ShowDeathScene(false);
                }
                else
                {
                    Debug.LogWarning("PlayerController component not found on player GameObject");
                }
            }
            else
            {
                Debug.LogWarning("Player GameObject not found");
            }
        }
    }
}

