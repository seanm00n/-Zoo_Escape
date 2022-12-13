using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuCanvas;
    public GameObject Menu;

    public void ClickStartBtn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ClickMainBtn()
    {
        SceneManager.LoadScene("MainScene");
    }

    void Start()
    {
       //Menu.SetActive(false);
    }

    void Update()
{
        if (Input.GetKeyDown(KeyCode.Escape))
        {  
            if (GameIsPaused)
        {
             Restart();
        }
            else
            {
            Pause();
            }
        }
}

    public void Restart()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }


    public void Pause()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void BackBtn()
    {
        Debug.Log("아직 미구현입니다...");
    }

    public void SoundBtn()
    {
        Debug.Log("아직 미구현입니다...");
        Application.Quit();
    }
}