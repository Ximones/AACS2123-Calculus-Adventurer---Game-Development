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
    public AudioClip buttonClickClip; // Changed from AudioSource to AudioClip

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
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
    }

    private void Quit()
    {
        Debug.Log("Quit method called");
        SceneManager.LoadScene("Main Menu");
    }

    private void PlayButtonClickSound()
    {
        if (buttonClickClip != null)
        {
            Debug.Log("Playing button click sound");
            AudioSource.PlayClipAtPoint(buttonClickClip, Camera.main.transform.position);
        }
        else
        {
            Debug.LogWarning("Button click sound clip is not assigned");
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
        if (collision.gameObject.CompareTag("Death"))
        {
            ShowDeathScene(false);
        }
    }
}