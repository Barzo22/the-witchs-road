using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadDefeat()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lose");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}