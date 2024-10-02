using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public GameObject setting;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {

            ToggleSetting();

        }
    }

    public void TogglePause()
    {
        Time.timeScale = 0.0f; // Pause the game
    }

    public void StopPause()
    {

        Time.timeScale = 1.0f; // Resume normal time
    }

    public void ToggleSetting()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            setting.SetActive(true);
            TogglePause();
        }
        else
        {
            setting.SetActive(false);
            StopPause();
        }
    }
}
