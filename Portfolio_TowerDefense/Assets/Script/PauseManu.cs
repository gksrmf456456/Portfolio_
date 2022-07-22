using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManu : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        rectTransform.SetSiblingIndex(1);
    }

    public void ClickPause()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void ClickRestart()
    {
        SceneManager.LoadScene(1);
    }

    public void ClickContinue()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void ClickExit()
    {
        Application.Quit();
    }

}
