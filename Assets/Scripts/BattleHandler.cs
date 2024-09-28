using System.Collections;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    public PlayerBehaviour playerBehaviour;
    public Enemy enemyBehaviour;  // Assuming this is your enemy behavior script
    public CalculusQuestion calculusQuestion;     // Your question-handling script

    private void Start()
    {
        // Start the battle loop when the game begins
        StartCoroutine(BattleLoop());
    }

    private IEnumerator BattleLoop()
    {
        // Continue looping until either the player or enemy health reaches zero
        while (playerBehaviour.playerHealth > 0 && enemyBehaviour.getEnemyHealth() > 0)
        {
            // Trigger a calculus question
            calculusQuestion.TriggerEnemy();

            // Wait for the player to answer the question
            yield return new WaitUntil(() => calculusQuestion.getPlayerAns() != 0);

            // Check if the player answered correctly
            if (calculusQuestion.getPlayerAns() == 1)
            {
                // Player answered correctly, so the player attacks
                playerBehaviour.WalkToTarget();
                calculusQuestion.playerAns = 0;  // Reset the answer state
            }
            
            if (calculusQuestion.getPlayerAns() == -1)
            {
                // Player answered incorrectly, so the enemy attacks
                enemyBehaviour.Move();
                calculusQuestion.playerAns = 0;
            }

            // Wait for both the player and enemy to stop moving before the next question
            yield return new WaitUntil(() => !playerBehaviour.isMoving && !enemyBehaviour.getMoving());
        }

        // Check the outcome of the battle and display a message
        if (playerBehaviour.playerHealth <= 0)
        {
            Debug.Log("Player has been defeated!");
        }
        else if (enemyBehaviour.getEnemyHealth() <= 0)
        {
            Debug.Log("Enemy has been defeated!");
        }
    }
}
