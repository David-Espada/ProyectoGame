using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject ui;

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        ui.SetActive(false);
        Time.timeScale = 0;
    }
    public void UnPauseGame()
    {
        pausePanel.SetActive(false);
        ui.SetActive(true);
        Time.timeScale = 1;
    }
}
