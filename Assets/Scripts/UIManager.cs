using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    public void SaveAndContinue() {
        //Save game
        //Load Next Level
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
 }
