using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CalculusQuestion : MonoBehaviour
{
   // public GameObject player;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI incorrectAnswerText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI enemyHealthText;

    public TMP_InputField answerInput;

    public Button submitButton;
    public Button backButton;

    public AudioSource buttonClickAudioSource;
    public GameObject questionPanel;
    public DeathSceneManager deathSceneManager;

    public PlayerBehaviour playerBehaviour;
    public Enemy enemyBehaviour;
    private List<Question> questions = new List<Question>();

    public RectTransform healthBar;
    public RectTransform enemyHealthBar;
    
    private int correctCoefficient;
    private int correctExponent;
    public int playerAns = 0;

    public GameObject deathScene;
    public GameObject victoryScene;

    void Start()
    {
        deathScene.SetActive(false);
        victoryScene.SetActive(false);
        submitButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(CheckAnswer)));
        InitializeQuestions();
        incorrectAnswerText.text = "";
        
    }

    private IEnumerator HandleButtonClick(System.Action action)
    {
        PlayButtonClickSound();
        yield return new WaitForSeconds(0.5f); 
        action.Invoke();
    }

    private void InitializeQuestions()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        switch (currentSceneName)
        {
            case "Forest":
                InitializePowerRuleQuestions();
                break;
            case "Cave":
            case "Cave 1":
                InitializeSumRuleQuestions();
                break;
            case "Dungeon":
                InitializeProductRuleQuestions();
                break;
            case "Dungeon 1":
                InitializeProductRuleQuestions();
                break;
            default:
                Debug.LogError("No questions set for this level.");
                break;
        }
    }

    private void InitializePowerRuleQuestions()
    {
        questions.Add(new Question("Use the power rule to find the derivative of y = x^15.\n What is the derivative of y?", 15, 14));
        questions.Add(new Question("Apply the power rule to find the derivative of y = x^99.\n What is the result?", 99, 98));
        questions.Add(new Question("Use the power rule to find the derivative of y = 7x^4.\n What is the derivative of y?", 28, 3));
        questions.Add(new Question("Apply the power rule to find the derivative of y = 2x^5.\n What is the result?", 10, 4));
        questions.Add(new Question("Use the power rule to find the derivative of y = 6x^6.\n What is the derivative of y?", 36, 5));
        questions.Add(new Question("Apply the power rule to find the derivative of y = 5x^7.\n What is the result?", 35, 6));
    }

    private void InitializeSumRuleQuestions()
    {
        questions.Add(new Question("What is the derivative of (3x^2 + 2x)?", 8, 1));
        questions.Add(new Question("What is the derivative of (4x^3 + x^2)?", 13, 2));
        questions.Add(new Question("What is the derivative of (7x^4 + 3x^3)?", 31, 3));
        questions.Add(new Question("What is the derivative of (2x^5 + 5x^2)?", 15, 4));
        questions.Add(new Question("What is the derivative of (6x^6 + 4x^3)?", 40, 5));
        questions.Add(new Question("What is the derivative of (5x^7 + 2x^4)?", 37, 6));
    }

    private void InitializeProductRuleQuestions()
    {
        questions.Add(new Question("What is the derivative of (3x^2 * 2x)?", 12, 2));
        questions.Add(new Question("What is the derivative of (4x^3 * x^2)?", 20, 4));
        questions.Add(new Question("What is the derivative of (7x^4 * 3x^3)?", 63, 7));
        questions.Add(new Question("What is the derivative of (2x^5 * 5x^2)?", 40, 7));
        questions.Add(new Question("What is the derivative of (6x^6 * 4x^3)?", 144, 9));
        questions.Add(new Question("What is the derivative of (5x^7 * 2x^4)?", 70, 11));
    }

    public void TriggerEnemy()
    {
        AskCalculusQuestion();
    }

    private void AskCalculusQuestion()
    {
        int randomIndex = Random.Range(0, questions.Count);
        Question selectedQuestion = questions[randomIndex];
        questionText.text = selectedQuestion.questionText;
        correctCoefficient = selectedQuestion.correctCoefficient;
        correctExponent = selectedQuestion.correctExponent;
        questionPanel.SetActive(true);
        answerInput.text = "";
        incorrectAnswerText.text = "";
        FocusInputField(); // Focus the input field
        
    }

    public void CheckAnswer()
    {
        string[] answerParts = answerInput.text.Split('x');
        if (answerParts.Length == 2 && int.TryParse(answerParts[0], out int coefficient) && int.TryParse(answerParts[1], out int exponent))
        {
            if (coefficient == correctCoefficient && exponent == correctExponent)
            {

                incorrectAnswerText.text = "Correct.You are smarter than i think !!";
                playerAns = 1;
            }
            else
            {
                incorrectAnswerText.text = "Incorrect answer.Try Harder Noob";
                playerAns = -1;
            }
            
        }
        else
        {
            incorrectAnswerText.text = "Invalid answer format.Answer better please";
            playerAns = -1;
        }
           
    }


    private void PlayButtonClickSound()
    {
        if (buttonClickAudioSource != null && buttonClickAudioSource.clip != null)
        {
            buttonClickAudioSource.Play();
        }
    }

    public void UpdateHealthText()
    {
        string enemyName = enemyBehaviour.getEnemyName();
        float enemyHealth = enemyBehaviour.getEnemyHealth();

        float maxHealth = 100f; // maximum health is 100

        float playerHealthPercent = playerBehaviour.playerHealth / maxHealth;
        float enemyHealthPercent = enemyHealth / maxHealth;

        healthBar.localScale = new Vector3(playerHealthPercent * 200, healthBar.localScale.y, healthBar.localScale.z); // Scale only on X-axis
        enemyHealthBar.localScale = new Vector3(enemyHealthPercent * 200, enemyHealthBar.localScale.y, enemyHealthBar.localScale.z); // Scale only on X-axis

        healthText.text = "Player : " + playerBehaviour.playerHealth + "/100";
        enemyHealthText.text = enemyName + " : " + enemyHealth + "/100";

        if (playerBehaviour.playerHealth <= 0)
        {
            deathSceneActive();
        }

        // if (enemyBehaviour.getEnemyHealth() <= 0)
        // {
        //     victorySceneActive();
        // }
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

        victoryScene.SetActive(false);
    }



    private void FocusInputField()
    {
        EventSystem.current.SetSelectedGameObject(answerInput.gameObject, null); // Set the selected game object
        answerInput.OnPointerClick(new PointerEventData(EventSystem.current)); // If needed, simulate a click to focus
    }

    public void OnInputFieldClick()
    {
        FocusInputField();
    }

    public int getPlayerAns()
    {
        return playerAns;
    }


}

[System.Serializable]
public class Question
{
    public string questionText;
    public int correctCoefficient;
    public int correctExponent;

    public Question(string questionText, int correctCoefficient, int correctExponent)
    {
        this.questionText = questionText;
        this.correctCoefficient = correctCoefficient;
        this.correctExponent = correctExponent;
    }
}