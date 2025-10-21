using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("hasStarted"))
        {
            PlayerPrefs.SetInt("hasStarted", 0);
            PlayerPrefs.Save();
            Debug.Log("Primera vez que se inicia el juego");
        }
        else
        {
          Debug.Log("No es la primera vez que se inicia el juego");
        }
    }
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
