using UnityEngine;
using UnityEngine.InputSystem; // Necesario para el nuevo Input System
using UnityEngine.UI; // Necesario si usas UI.Image

public class PauseMenuController : MonoBehaviour
{
    [Header("UI del Menú de Pausa")]
    [Tooltip("El GameObject del panel de pausa/inventario que se activará/desactivará.")]
    public GameObject pausePanel; // Arrastra tu PausePanel/InventoryPanel aquí

    private bool isPaused = false;

    void Start()
    {
        // Asegurarse de que el panel esté oculto al inicio
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("PauseMenuController: No se ha asignado el GameObject del panel de pausa. Por favor, arrástralo en el Inspector.");
            enabled = false; // Desactiva el script si no hay panel
        }

        // Asegurarse de que el juego no esté pausado al inicio si Start() se llama después de un reinicio
        Time.timeScale = 1f;
    }

    // Método llamado por el Input System cuando se presiona la tecla asignada
    // Debes vincular este método a tu acción "TogglePause" en el PlayerInput.
    public void OnTogglePause(InputAction.CallbackContext context)
    {
        // Solo reacciona al momento en que se presiona la tecla (Started)
        if (context.started) 
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true); // Muestra el panel
        Time.timeScale = 0f; // Detiene completamente el tiempo en el juego
        // Opcional: Bloquear el cursor del ratón
        // Cursor.lockState = CursorLockMode.None; // Mostrar cursor
        // Cursor.visible = true; // Hacerlo visible
    }

    void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false); // Oculta el panel
        Time.timeScale = 1f; // Reanuda el tiempo a velocidad normal
        // Opcional: Volver a bloquear el cursor del ratón
        // Cursor.lockState = CursorLockMode.Locked; // Ocultar cursor y centrarlo
        // Cursor.visible = false; // Hacerlo invisible
    }

    // Puedes añadir un método para salir del juego, por ejemplo
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
        // Para el editor, puedes usar: UnityEditor.EditorApplication.isPlaying = false;
    }
}