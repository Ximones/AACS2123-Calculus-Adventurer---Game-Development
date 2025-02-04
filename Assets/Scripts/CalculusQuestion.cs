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

    public GameObject hintImage;
    public int hintCount = 0;
    public Button submitButton;
    public Button hintButton;

    public AudioSource buttonClickAudioSource;
    public GameObject questionPanel;
    //public DeathSceneManager deathSceneManager;

    public PlayerBehaviour playerBehaviour;
    public Enemy enemyBehaviour;

    public RectTransform healthBar;
    public RectTransform enemyHealthBar;

    private string correctAnswer;
    private List<Question> questions = new List<Question>();
    public int playerAns = 0;

    private int currentQuestionIndex = -1;

    void Start()
    {
        hintImage.SetActive(false);
        hintButton.onClick.AddListener(() => StartCoroutine(HandleButtonClick(showHints)));
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
            case "Dungeon 1":
            case "Dungeon 2":
            case "Dungeon 3":
                InitializeProductRuleQuestions();
                break;
            default:
                Debug.LogError("No questions set for this level.");
                break;
        }
    }

    private void InitializePowerRuleQuestions()
    {
        questions.Add(new Question("1.Use the power rule to find the derivative of y = x^15.\n What is the derivative of y?", "15x^14"));
        questions.Add(new Question("2.Apply the power rule to find the derivative of y = x^99.\n What is the result?", "99x^98"));
        questions.Add(new Question("3.Use the power rule to find the derivative of y = 7x^4.\n What is the derivative of y?", "28x^3"));
        questions.Add(new Question("4.Apply the power rule to find the derivative of y = 2x^5.\n What is the result?", "10x^4"));
        questions.Add(new Question("5.Use the power rule to find the derivative of y = 6x^6.\n What is the derivative of y?", "36x^5"));
        questions.Add(new Question("6.Apply the power rule to find the derivative of y = 5x^7.\n What is the result?", "35x^6"));
        questions.Add(new Question("7.Use the power rule to find the derivative of y = x^20.\n What is the derivative of y?", "20x^19"));
        questions.Add(new Question("8.Apply the power rule to find the derivative of y = 3x^10.\n What is the result?", "30x^9"));
        questions.Add(new Question("9.Use the power rule to find the derivative of y = 9x^8.\n What is the derivative of y?", "72x^7"));
        questions.Add(new Question("10.Apply the power rule to find the derivative of y = 12x^3.\n What is the result?", "36x^2"));
        questions.Add(new Question("11.Use the power rule to find the derivative of y = 5x^11.\n What is the derivative of y?", "55x^10"));
        questions.Add(new Question("12.Use the power rule to find the derivative of y = x^12.\n What is the derivative of y?", "12x^11"));
        questions.Add(new Question("13.Apply the power rule to find the derivative of y = x^50.\n What is the result?", "50x^49"));
        questions.Add(new Question("14.Use the power rule to find the derivative of y = 8x^9.\n What is the derivative of y?", "72x^8"));
        questions.Add(new Question("15.Apply the power rule to find the derivative of y = 3x^4.\n What is the result?", "12x^3"));
        questions.Add(new Question("16.Use the power rule to find the derivative of y = 5x^7.\n What is the derivative of y?", "35x^6"));
        questions.Add(new Question("17.Apply the power rule to find the derivative of y = 4x^3.\n What is the result?", "12x^2"));
        questions.Add(new Question("18.Use the power rule to find the derivative of y = 10x^6.\n What is the derivative of y?", "60x^5"));
        questions.Add(new Question("19.Use the power rule to find the derivative of y = x^25.\n What is the derivative of y?", "25x^24"));
        questions.Add(new Question("20.Apply the power rule to find the derivative of y = 6x^5.\n What is the result?", "30x^4"));
    }

    private void InitializeSumRuleQuestions()
    {
        questions.Add(new Question("1.What is the derivative of y = 4x^2 + 5x^3?", "8x + 15x^2"));
        questions.Add(new Question("2.What is the derivative of y = 6x^2 - 3x^3?", "12x - 9x^2"));
        questions.Add(new Question("3.What is the derivative of y = (2x^2 - x)^2?", "16x^3 - 12x^2 + 2x"));
        questions.Add(new Question("4.What is the derivative of y = x^10 - 5x^7 + 4x^5 - 2x^2 + 10x?", "10x^9 - 35x^6 + 20x^4 - 4x + 10"));
        questions.Add(new Question("5.What is the derivative of y = 3x^4 + 7x^2?", "12x^3 + 14x"));
        questions.Add(new Question("6.What is the derivative of y = 5x^5 - 8x^3 + 6x?", "25x^4 - 24x^2 + 6"));
        questions.Add(new Question("7.What is the derivative of y = x^6 - 4x^2 + 3?", "6x^5 - 8x"));
        questions.Add(new Question("8.What is the derivative of y = 2x^7 + 3x^5 - x^3 + 5?", "14x^6 + 15x^4 - 3x^2"));
        questions.Add(new Question("9.What is the derivative of y = 6x^3 + 4x^2 - 2x?", "18x^2 + 8x - 2"));
        questions.Add(new Question("10.What is the derivative of y = 4x^3 + 7x^2?", "12x^2 + 14x"));
        questions.Add(new Question("11.What is the derivative of y = 5x^6 - 3x^4?", "30x^5 - 12x^3"));
        questions.Add(new Question("12.What is the derivative of y = 2x^8 + 4x^3?", "16x^7 + 12x^2"));
        questions.Add(new Question("13.What is the derivative of y = 9x^4 - 6x^2 + x?", "36x^3 - 12x + 1"));
        questions.Add(new Question("14.What is the derivative of y = 3x^5 + 2x^3 - x?", "15x^4 + 6x^2 - 1"));
        questions.Add(new Question("15.What is the derivative of y = x^7 + 5x^2 - 4x + 7?", "7x^6 + 10x - 4"));
        questions.Add(new Question("16.What is the derivative of y = 8x^4 - 3x^2 + 6?", "32x^3 - 6x"));
        questions.Add(new Question("17.What is the derivative of y = x^5 - 5x^3 + 9x?", "5x^4 - 15x^2 + 9"));
        questions.Add(new Question("18.What is the derivative of y = 2x^6 + 4x^2 - 7x + 12?", "12x^5 + 8x - 7"));
        questions.Add(new Question("19.What is the derivative of y = 7x^3 + 4x^2 + 5x?", "21x^2 + 8x + 5"));
    }


    private void InitializeProductRuleQuestions()
    {
        questions.Add(new Question("1.What is the derivative of y = (4x^2 + 2)(3x^3 - 1)?", "60x^4 + 18x^2 - 8x"));
        questions.Add(new Question("2.What is the derivative of y = (2 - x^3)(3x^3 + x - 1)?", "-6x^5 - 4x^3 + 9x^2 + 2"));
        questions.Add(new Question("3.What is the derivative of y = (x^2 + 1)(x - 5 - 1/x)?", "3x^2 - 10x + 1/x^2"));
        questions.Add(new Question("4.What is the derivative of y = (x - 1/x)(x^2 - 1/x - 1)?", "3x^2 - 2 - 1/x^2 - 2/x^3"));
        questions.Add(new Question("5.What is the derivative of y = (x^2 + 3)(2x^4 - x)?", "10x^5 - 2x^3 - 3x + 3"));
        questions.Add(new Question("6.What is the derivative of y = (x^3 + 2)(4x^2 - 3)?", "12x^4 + 8x^2 - 9x"));
        questions.Add(new Question("7.What is the derivative of y = (x + 1)(x^2 - x + 2)?", "3x^2 - 2x + 3"));
        questions.Add(new Question("8.What is the derivative of y = (x^4 + 1)(x^3 - 1)?", "7x^6 - x^3 - 1"));
        questions.Add(new Question("9.What is the derivative of y = (x^5 - 2)(x^4 + x - 3)?", "9x^8 + x^5 - 12x^3 - x + 6"));
        questions.Add(new Question("10.What is the derivative of y = (x^3 + 2)(x^4 - 1)?", "7x^6 + 4x^3 - 2x^2"));
        questions.Add(new Question("11.What is the derivative of y = (3x^2 - 5)(2x^3 + 1)?", "18x^4 - 30x^2 + 6x"));
        questions.Add(new Question("12.What is the derivative of y = (2x^3 + 3)(4x^2 - 5)?", "24x^4 - 30x^2 + 8x"));
        questions.Add(new Question("13.What is the derivative of y = (x^5 + 2)(x^2 - 3)?", "7x^6 - 10x^2"));
        questions.Add(new Question("14.What is the derivative of y = (2x^2 + 1)(x^3 - 2)?", "8x^4 - 4x"));
        questions.Add(new Question("15.What is the derivative of y = (x^3 + 4x)(x^2 - 1)?", "5x^4 + 4x^2 - 2x"));
        questions.Add(new Question("16.What is the derivative of y = (x^4 - 1)(2x^3 + 5)?", "8x^6 + 10x^3"));
        questions.Add(new Question("17.What is the derivative of y = (x^5 + 3)(x^2 + 1)?", "7x^6 + 5x^2"));
        questions.Add(new Question("18.What is the derivative of y = (2x^4 - 3x)(3x^3 + 2)?", "24x^6 + 12x^2"));
        questions.Add(new Question("19.What is the derivative of y = (x^3 + 2)(x^2 + 3)?", "5x^5 + 6x^2"));
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
            playerAns = -1;
        }
    }

    public void showHints()
    {

        if(hintCount % 2 == 0)
        {
            
            hintImage.SetActive(true);
        }
        else
        {
            hintImage.SetActive(false);
        }

        hintCount++;
       
    }

    // Validate input regex
    private bool IsValidInput(string input)
    {
        // Regex to allow only digits, x, y, +, -, *, and ^
        return Regex.IsMatch(input, @"^[0-9xy\+\-\*\/\^]+$");
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

        //get random number index
        System.Random random = new System.Random();
        currentQuestionIndex = random.Next(0, questions.Count);
        
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