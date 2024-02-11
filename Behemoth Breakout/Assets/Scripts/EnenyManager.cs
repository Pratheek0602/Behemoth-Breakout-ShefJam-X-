using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public int totalEnemies = 3; // Set the total number of enemies here
    private int enemiesKilled = 0;

    // Method to be called when an enemy is killed
    public void EnemyKilled()
    {
        enemiesKilled++;
        Debug.Log("ENEMY KILLEDDDDDDDDDDDDD");

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
        SceneManager.LoadScene("Game Clear", LoadSceneMode.Single);
    }
}
