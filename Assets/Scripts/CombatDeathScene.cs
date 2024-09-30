using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
public class CombatDeathScene : MonoBehaviour
{
    [SerializeField]
    public string restartLevel;

    public AudioClip SFX;
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
    IEnumerator waitRestart()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(restartLevel);
    }

    public void Restart()
    {
        audioSrc.PlayOneShot(SFX);
        StartCoroutine(waitRestart());
    }
    IEnumerator waitQuit()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Main Menu");
    }
    public void Quit()
    {
        audioSrc.PlayOneShot(SFX);
        StartCoroutine(waitQuit());
    }
}
