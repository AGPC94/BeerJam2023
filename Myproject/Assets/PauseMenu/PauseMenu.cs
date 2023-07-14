using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseWindow;
    [SerializeField] float delayTime = .5f;
    public bool IsPaused;
    public static PauseMenu instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void TogglePause()
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        StartCoroutine(DelayResume());
    }

    IEnumerator DelayResume()
    {
        yield return new WaitForSecondsRealtime(delayTime);
        Time.timeScale = 1;
        IsPaused = false;
        pauseWindow.SetActive(false);
        print("Resume");
    }

    public void Pause()
    {
        pauseWindow.SetActive(true);
        Time.timeScale = 0;
        IsPaused = true;
        print("Pause");
    }

    public void Quit()
    {
        SceneManager.LoadScene("MainMenu");
        print("Quit");
    }

}
