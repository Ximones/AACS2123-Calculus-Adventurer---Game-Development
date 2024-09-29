using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider backgroundVolumeSlider;  //Option Audio Settings
    public AudioMixer backgroundMixer;
    private float backgroundValue;

    // Start is called before the first frame update
    public void Start()
    {
        //Set for Audio Value for avoid value back to default
        backgroundMixer.GetFloat("Background", out backgroundValue);
        backgroundVolumeSlider.value = backgroundValue;
    }

    public void SetVolume()
    {
        backgroundMixer.SetFloat("Background", backgroundVolumeSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
