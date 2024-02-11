using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public int totalEnemies = 1; // Set the total number of enemies here
    private int enemiesKilled = 0;

    // Method to be called when an enemy is killed
    public void EnemyKilled()
    {
        enemiesKilled++;

        // Check if all enemies are killed
        if (enemiesKilled >= totalEnemies)
        {
            // Trigger game over screen
            Debug.Log("Game Over");
            // Call a method to show game over screen
            ShowGameOverScreen();
        }
    }

    // Method to show the game over screen
    private void ShowGameOverScreen()
    {
        // Code t
}
}
