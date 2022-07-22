using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void GameOverImage()
    {
        gameObject.SetActive(true);
    }

    public void ClickRestart()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }

    public void ClickBackToManu()
    {
        SceneManager.LoadScene(0);
    }
}
