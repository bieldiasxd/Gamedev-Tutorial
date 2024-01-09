using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;

    public void gameOver() {
        gameOverUI.SetActive(true);
    }

    public void restart(){
        Score.scoreCount = 0;
        // Obtém o nome da cena atual
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Recarrega a cena atual
        SceneManager.LoadScene(currentSceneName);
    }

    public void quit(){
        Application.Quit();
    }
}
