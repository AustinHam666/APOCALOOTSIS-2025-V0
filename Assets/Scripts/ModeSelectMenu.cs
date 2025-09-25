using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectMenu : MonoBehaviour
{
    // Llamado desde el botón "Modo Cooperativo"
    public void PlayCoop()
    {
        SceneManager.LoadScene("SampleScene"); // Cambiá "SampleScene" por el nombre real de tu escena
    }

    // Llamado desde el botón "Modo Versus"
    public void PlayVersus()
    {
        // Si todavía no tenés la escena de Versus, podés duplicar GameScene
        // y adaptarla más adelante.
        SceneManager.LoadScene("VersusScene");
    }

    // Llamado desde el botón "Atrás"
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
