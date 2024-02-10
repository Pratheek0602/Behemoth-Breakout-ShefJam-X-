using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public void ExitButton(){
        SceneManager.LoadScene("Main Menu");
        // Application.Quit();
        // Debug.Log("Main Menu");

    }
    public void RestartButton(){
        SceneManager.LoadScene("Game Scene");

    }
}
