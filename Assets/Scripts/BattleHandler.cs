using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleHandler : MonoBehaviour
{
    public AudioSource buttonClickAudioSource;
    public Button returnLevelButton;
    public PlayerBehaviour playerBehaviour;
    public Enemy enemyBehaviour;  // Assuming this is your enemy behavior script

    public CalculusQuestion calculusQuestion;     // Your question-handling script
    public TextMeshProUGUI damageText;
    public GameObject deathScene;
    public GameObject victoryScene;
    public GameObject damageObject;

    public GameObject displayCalculus;
    public GameObject inputCalculus;
    public GameObject confirmButton;

    public Animator damageAnimator;
    private bool isShowingDamage = false;

    private void Start()
    {
        damageText.text = "";
        deathScene.SetActive(false);
        victoryScene.SetActive(false);

        returnLevelButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(EndCombat)));

        if (damageObject == null)
        {
            damageObject = GameObject.FindGameObjectWithTag("Damage");
        }
        else
        {
            Debug.Log("Found damage Object");
        }

        if (damageAnimator == null)
        {
            damageAnimator = damageObject.GetComponent<Animator>();
        }
        else
        {
            Debug.Log("Found damage Animator");
        }

        // Start the battle loop when the game begins
        StartCoroutine(BattleLoop());
    }

    private IEnumerator BattleLoop()
    {
        // Continue looping until either the player or enemy health reaches zero
        while (playerBehaviour.playerHealth > 0 && enemyBehaviour.getEnemyHealth() > 0)
        {
            calculusQuestion.UpdateHealthText();
            // Trigger a calculus question
            calculusQuestion.TriggerEnemy();

            // Wait for the player to answer the question
            yield return new WaitUntil(() => calculusQuestion.getPlayerAns() != 0);

            // Check if the player answered correctly
            if (calculusQuestion.getPlayerAns() == 1)
            {
                displayCalculus.SetActive(false);
                inputCalculus.SetActive(false);
                confirmButton.SetActive(false);
                // Player answered correctly, so the player attacks
                playerBehaviour.WalkToTarget();
                
                calculusQuestion.playerAns = 0;  // Reset the answer state
            }
            
            if (calculusQuestion.getPlayerAns() == -1)
            {
                displayCalculus.SetActive(false);
                inputCalculus.SetActive(false);
                confirmButton.SetActive(false);
                // Player answered incorrectly, so the enemy attacks
                enemyBehaviour.Move();
                calculusQuestion.playerAns = 0;
            }

            // Wait for both the player and enemy to stop moving before the next question

            yield return new WaitUntil(() => !playerBehaviour.isMoving && !enemyBehaviour.getMoving());

            displayCalculus.SetActive(true);
            inputCalculus.SetActive(true);
            confirmButton.SetActive(true);
        }

            
            
            calculusQuestion.UpdateHealthText();
        // Check the outcome of the battle and display a message
        if (playerBehaviour.playerHealth <= 0)
        {
            Debug.Log("Player has been defeated!");
            deathSceneActive();
        }
        else if (enemyBehaviour.getEnemyHealth() <= 0)
        {
            Debug.Log("Enemy has been defeated!");
            victorySceneActive();
            
        }

    }

    private void deathSceneActive()
    {
        StartCoroutine(ActivateDeathSceneAfterDelay());
    }

    private IEnumerator ActivateDeathSceneAfterDelay()
    {
        yield return new WaitForSeconds(1.2f);

        deathScene.SetActive(true);
    }
    private void victorySceneActive()
    {
        StartCoroutine(ActivateVictorySceneAfterDelay());
    }

    private IEnumerator ActivateVictorySceneAfterDelay()
    {
        yield return new WaitForSeconds(1.2f);

        victoryScene.SetActive(true);
    }
    private IEnumerator HandleButtonClick(System.Action action)
    {
        PlayButtonClickSound();
        yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds
        action.Invoke();
    }
    private void PlayButtonClickSound()
    {
        if (buttonClickAudioSource != null && buttonClickAudioSource.clip != null)
        {
            buttonClickAudioSource.Play();
        }
    }
    public void EndCombat()
    {
        GameManager.Instance.ReturnToLevel();
    }

    public void UpdateDamageText(float damageShow, bool isCrit)
    {
        if (isShowingDamage)
            return; // Avoid running the damage text animation if it's already showing

        isShowingDamage = true;  // Set flag to indicate damage text is active
        damageText.text = "";     // Clear the text first

        damageObject.SetActive(true);  // Make the damage object visible
        damageAnimator.SetInteger("AnimState", 1);  // Start animation

        if (isCrit)
        {
            damageText.color = new Color32(255, 13, 13, 255); // FF0D0D color (red)
            damageText.text = "*CRITICAL " + damageShow + " DAMAGE*";
        }
        else
        {
            damageText.color = Color.white; // Set to original color (white)
            damageText.text = "*" + damageShow + " DAMAGE*";
        }

        // Start coroutine to wait for the animation to finish and reset the state
        StartCoroutine(WaitPopFinish());
    }

    private IEnumerator WaitPopFinish()
    {
        // Wait for 1 second before resetting the animation and text
        yield return new WaitForSeconds(1);

        damageText.text = "";  // Clear the text
        damageAnimator.SetInteger("AnimState", 0);  // Reset the animation to idle
        damageObject.SetActive(false);  // Hide the damage object
        isShowingDamage = false;  // Reset flag to allow future damage texts
    }
}
