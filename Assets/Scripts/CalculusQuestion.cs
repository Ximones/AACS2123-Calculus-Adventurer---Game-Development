using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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

    public RectTransform healthBar;
    public RectTransform enemyHealthBar;

    private string correctAnswer;
    private List<Question> questions = new List<Question>();
    public int playerAns = 0;

    public GameObject deathScene;
    public GameObject victoryScene;

    private int currentQuestionIndex = -1;

    void Start()
    {
        deathScene.SetActive(false);
        victoryScene.SetActive(false);
        submitButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(CheckAnswer)));
        InitializeQuestions();
        incorrectAnswerText.text = "";
        AskCalculusQuestion(); // Initialize the first question

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
            default:
                Debug.LogError("No questions set for this level.");
                break;
        }
    }

    private void InitializePowerRuleQuestions()
    {
        questions.Add(new Question("Use the power rule to find the derivative of y = x^15.\n What is the derivative of y?", "15x^14"));
        questions.Add(new Question("Apply the power rule to find the derivative of y = x^99.\n What is the result?", "99x^98"));
        questions.Add(new Question("Use the power rule to find the derivative of y = 7x^4.\n What is the derivative of y?", "28x^3"));
        questions.Add(new Question("Apply the power rule to find the derivative of y = 2x^5.\n What is the result?", "10x^4"));
        questions.Add(new Question("Use the power rule to find the derivative of y = 6x^6.\n What is the derivative of y?", "36x^5"));
        questions.Add(new Question("Apply the power rule to find the derivative of y = 5x^7.\n What is the result?", "35x^6"));
    }

    private void InitializeSumRuleQuestions()
    {
        questions.Add(new Question("What is the derivative of y = 4x^2 + 5x^3?", "8x + 15x^2"));
        questions.Add(new Question("What is the derivative of y = 6x^2 - 3x^3?", "12x - 9x^2"));
        questions.Add(new Question("What is the derivative of y = (2x^2 - x)^2?", "16x^3 - 12x^2 + 2x"));
        questions.Add(new Question("What is the derivative of y = x^10 - 5x^7 + 4x^5 - 2x^2 + 10x?", "10x^9 - 35x^6 + 20x^4 - 4x + 10"));
    }

    private void InitializeProductRuleQuestions()
    {
        questions.Add(new Question("What is the derivative of y = (4x^2 + 2)(3x^3 - 1)?", "60x^4 + 18x^2 - 8x"));
        questions.Add(new Question("What is the derivative of y = (2 - x^3)(3x^3 + x - 1)?", "-6x^5 - 4x^3 + 9x^2 + 2"));
        questions.Add(new Question("What is the derivative of y = (x^2 + 1)(x - 5 - 1/x)?", "3x^2 - 10x + 1/x^2"));
        questions.Add(new Question("What is the derivative of y = (x - 1/x)(x^2 - 1/x - 1)?", "3x^2 - 2 - 1/x^2 - 2/x^3"));
    }

    // Check answer method
    public void CheckAnswer()
    {
        string userAnswer = answerInput.text.Replace(" ", ""); // Remove spaces

        if (IsValidInput(userAnswer))
        {
            string normalizedUserAnswer = NormalizeExpression(userAnswer);
            string normalizedCorrectAnswer = NormalizeExpression(correctAnswer);

            if (normalizedUserAnswer == normalizedCorrectAnswer)
            {
                incorrectAnswerText.text = "Correct!";
                playerAns = 1;
                // Show the next question only if the answer is correct
                AskCalculusQuestion();
            }
            else
            {
                incorrectAnswerText.text = "Incorrect answer. Try again!";
                playerAns = -1;
                UpdateHealthText();
            }
        }
        else
        {
            incorrectAnswerText.text = "Invalid characters in input. Please use only numbers, x, y, +, -, *, ^.";
        }
    }

    // Validate input regex
    private bool IsValidInput(string input)
    {
        // Regex to allow only digits, x, y, +, -, *, and ^
        return Regex.IsMatch(input, @"^[0-9xy\+\-\*\^]+$");
    }

    // Normalize expressions
    private string NormalizeExpression(string expression)
    {
        // Remove spaces
        expression = expression.Replace(" ", "");

        // Standardize multiplication symbol
        expression = expression.Replace("*", "");

        // Split terms by + and - while preserving the operators
        string[] terms = Regex.Split(expression, @"(?=[+-])");

        // Sort terms in a way that makes mathematical sense
        Array.Sort(terms, (a, b) =>
        {
            // Extract the power of x for each term
            int powerA = GetPowerOfX(a);
            int powerB = GetPowerOfX(b);

            // Sort by power in descending order
            return powerB.CompareTo(powerA);
        });

        return string.Join("", terms);
    }

    private int GetPowerOfX(string term)
    {
        // Match the power of x in the term
        Match match = Regex.Match(term, @"x\^(\d+)");
        if (match.Success)
        {
            return int.Parse(match.Groups[1].Value);
        }
        else if (term.Contains("x"))
        {
            // If the term contains x but no explicit power, it's x^1
            return 1;
        }
        else
        {
            // If the term does not contain x, it's x^0
            return 0;
        }
    }

    public void TriggerEnemy()
    {
        AskCalculusQuestion();
    }

    private void AskCalculusQuestion()
    {
        currentQuestionIndex = (currentQuestionIndex + 1) % questions.Count;
        Question selectedQuestion = questions[currentQuestionIndex];
        questionText.text = selectedQuestion.questionText;
        correctAnswer = selectedQuestion.correctAnswer;
        questionPanel.SetActive(true);
        answerInput.text = "";
        incorrectAnswerText.text = "";
        FocusInputField(); // Focus the input field
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
    public string correctAnswer;

    public Question(string questionText, string correctAnswer)
    {
        this.questionText = questionText;
        this.correctAnswer = correctAnswer;
    }
}