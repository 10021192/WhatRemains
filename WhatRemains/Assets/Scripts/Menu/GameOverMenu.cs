using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverMenu;

    public static bool isGameOver;

    void Start()
    {
        gameOverMenu.SetActive(false);
    }

    public void ShowGameOverMenu()
    {
        Time.timeScale = 0f;
        gameOverMenu.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        gameOverMenu.SetActive(false);
        GameManager.Instance.Player.PlayerHealth.ResetHealth();
        GameManager.Instance.Player.PlayerMana.ResetMana();
        SceneManager.LoadScene("Game");
    }

    public void ReturnMainMenu()
    {
        Time.timeScale = 1f;
        gameOverMenu.SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }
}
