using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void ExitButton(){
        Application.Quit();
        Debug.Log("Game closed");

    }
    public void StartButton(){
        SceneManager.LoadScene("Story Scene");

    }
}
