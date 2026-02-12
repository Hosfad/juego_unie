using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    public Button playAgainButton; 

    void Start()
    {
        
        if (playAgainButton != null)
        {
            playAgainButton.onClick.AddListener(PlayAgain);
        }
        else
        {
            Debug.LogError("PlayAgainButton is not assigned in the WinMenu Inspector!");
        }
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }
}