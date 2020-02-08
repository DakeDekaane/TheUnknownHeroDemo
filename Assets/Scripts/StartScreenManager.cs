using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene("Chapter1");
    }
    
    public void QuitGame(){
        Application.Quit();
    }
}
