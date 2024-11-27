
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public void ToMainGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
        Time.timeScale = 1;
    }
    public void Quit()
    {
        Debug.Log("salio");
        Application.Quit();
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
