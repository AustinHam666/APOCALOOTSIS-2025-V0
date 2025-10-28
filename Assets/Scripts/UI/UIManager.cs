using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic; 
using UnityEngine.SceneManagement; 

public class UIManager : MonoBehaviour
{
    // --- SECCIÓN 1: Vidas y Corazones ---
    
    [Header("UI de Vidas")]
    public List<GameObject> corazones; 
    private int vidas;

    // --- SECCIÓN 2: Paneles de Menú ---
    [Header("Paneles de Menú")]
    public GameObject MainMenuPanel;
    public GameObject SettingsPanel;
    public GameObject ControlsPanel;
    
    // --- SECCIÓN 3: Referencias de Juego ---
    [Header("Referencias de Juego")]
    public GameManager gameManager; // Para Game Over
    
    void Start()
    {
        // Inicializa las vidas
        if (corazones != null)
        {
            vidas = corazones.Count;
        } else {
            vidas = 0;
        }
        
        // Inicializa el menú (asumiendo que este script está en tu escena de menú)
        ShowMainMenu(); 
    } // <-- LLAVE DE CIERRE DE START()
    
    // --- FUNCIONES PÚBLICAS Y DE JUEGO (Van fuera de Start/Update) ---

    // --- Funciones de Vida ---
    public void QuitarCorazon()
    {
        if (vidas <= 0) return;

        vidas--;
        
        if (corazones.Count > vidas && corazones[vidas] != null)
        {
             corazones[vidas].SetActive(false);
        }

        if (vidas == 0)
        {
            Debug.Log("¡Game Over! Te has quedado sin corazones.");
            // Aquí podrías llamar a gameManager.EndGame();
        }
    }

    public int GetVidasActuales()
    {
        return vidas;
    }
    
    public void ReiniciarCorazones()
    {
        vidas = corazones.Count;
        foreach (GameObject corazon in corazones)
        {
            corazon.SetActive(true);
        }
    }

    // --- Funciones de Navegación del Menú ---
    
    public void ShowMainMenu()
    {
        // Asegúrate de que todos los paneles estén asignados antes de llamar a esto
        if (MainMenuPanel != null) MainMenuPanel.SetActive(true);
        if (SettingsPanel != null) SettingsPanel.SetActive(false);
        if (ControlsPanel != null) ControlsPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        if (MainMenuPanel != null) MainMenuPanel.SetActive(false);
        if (SettingsPanel != null) SettingsPanel.SetActive(true);
        if (ControlsPanel != null) ControlsPanel.SetActive(false);
    }

    public void ShowControls()
    {
        if (SettingsPanel != null) SettingsPanel.SetActive(false); // Oculta el panel anterior
        if (ControlsPanel != null) ControlsPanel.SetActive(true); // Muestra el panel de Controles
    }

    public void BackToSettings()
    {
        ShowSettings(); // Vuelve a mostrar la pantalla de Ajustes
    }
    
    // Función de ejemplo para iniciar el juego
    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

} // <-- LLAVE DE CIERRE FINAL DE LA CLASE