using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Este método carga la escena del juego
  public void PlayGame()
{
    SceneManager.LoadScene("ModeSelectMenu");
}


    // Este método más adelante puede abrir un panel de settings
    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    // Este método cierra la aplicación
    public void QuitGame()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
}
