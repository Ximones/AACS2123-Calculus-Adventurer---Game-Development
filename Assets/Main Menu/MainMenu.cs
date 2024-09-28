using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioClip buttonHItSounds;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Level 1");
    }

    public void playGame()
    {
        audioSrc.PlayOneShot(buttonHItSounds);
        StartCoroutine(Wait());
    }

    public void optionGame()
    {
        audioSrc.PlayOneShot(buttonHItSounds);
    }

    public void quitGame()
    {
        audioSrc.PlayOneShot(buttonHItSounds);
        Application.Quit();
    }
}
