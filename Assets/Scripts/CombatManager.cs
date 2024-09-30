using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public Enemy enemy;
    private void Update()
    {
        if (enemy.getEnemyHealth() <= 0)
        {
            EndCombat();
        }
    }

    private void EndCombat()
    {
        Debug.Log("Combat ended, returning to previous scene.");

        // Return to the previous scene and restore player state
        GameManager.Instance.ReturnToPreviousScene();
    }
}
