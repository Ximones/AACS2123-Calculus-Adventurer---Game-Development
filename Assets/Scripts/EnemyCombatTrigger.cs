using UnityEngine;

public class EnemyCombatTrigger : MonoBehaviour
{
    public GameObject enemy;
    public GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.MarkEnemyForDestruction(enemy);  // Mark the enemy for destruction
            gameManager.EnterCombatScene();  // Enter combat scene
        }
    }
}
