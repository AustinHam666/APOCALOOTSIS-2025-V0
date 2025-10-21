using UnityEngine;
using UnityEngine.SceneManagement; // para volver al menú

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [Header("Panel de Pausa")]
    public GameObject pauseMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // reanuda tiempo
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // congela tiempo
        GameIsPaused = true;
    }

    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // resetear tiempo antes de cambiar de escena
        SceneManager.LoadScene("MainMenu"); // nombre de tu escena de menú
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
