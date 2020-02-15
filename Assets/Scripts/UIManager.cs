using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject pausePanel;
    public void SaveAndContinue() {
        //Save game
        //Load Next Level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SaveAndReturn() {
        //Save game
        SceneManager.LoadScene("MainMenu");   
    }
    public void Return() {
        SceneManager.LoadScene("MainMenu");
    }
    public void Retry() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Continue() {
        pausePanel.SetActive(false);
        TurnManager.instance.pause = false;
    }
    public void ShowMenu() {
        pausePanel.SetActive(true);
        TurnManager.instance.pause = true;
    }
 }
