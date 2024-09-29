using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioClip buttonHItSounds;
    static AudioSource audioSrc;

    public Slider backgroundVolumeSlider;  //Option Audio Settings
    public AudioMixer backgroundMixer;
    private float backgroundValue;
 
    public void Start()
    {

        //Set for Audio Value for avoid value back to default
        backgroundMixer.GetFloat("Background",out backgroundValue);
        backgroundVolumeSlider.value = backgroundValue;
  
        audioSrc = GetComponent<AudioSource>();
    }

    //Set Audio Value by Slider
    public void SetVolume()
    {
        backgroundMixer.SetFloat("Background",backgroundVolumeSlider.value);
    }

    void Update()
    {
        
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
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
