using UnityEngine;
using System.Collections; // Necesario para Coroutines

public class RoundStartDisplay : MonoBehaviour
{
    [Header("Configuración del Panel de Inicio")]
    [Tooltip("El GameObject del panel que se mostrará al inicio y luego se ocultará.")]
    public GameObject introPanel; // Arrastra tu IntroPanel (el hijo del Canvas) aquí

    [Tooltip("Tiempo en segundos que el panel de introducción estará visible.")]
    public float displayDuration = 3f; // Por ejemplo, 3 segundos

    [Header("Comportamiento de Pausa")]
    [Tooltip("¿El juego debe pausarse mientras se muestra el panel?")]
    public bool pauseGameWhileDisplaying = true;

    private void Start()
    {
        // Asegurarse de que el panel esté asignado
        if (introPanel == null)
        {
            Debug.LogError("RoundStartDisplay: No se ha asignado el GameObject del panel de introducción. Por favor, arrástralo en el Inspector.");
            enabled = false; // Desactiva el script
            return;
        }

        // Empezar la secuencia de muestra del panel
        StartCoroutine(ShowAndHideIntroPanel());
    }

    IEnumerator ShowAndHideIntroPanel()
    {
        // 1. Mostrar el panel (si no está activo)
        introPanel.SetActive(true);
        Debug.Log("Mostrando panel de introducción...");

        // 2. Pausar el juego si está configurado
        if (pauseGameWhileDisplaying)
        {
            Time.timeScale = 0f; // Pausa el juego
        }

        // 3. Esperar la duración especificada
        yield return new WaitForSecondsRealtime(displayDuration); // Usa WaitForSecondsRealtime para que funcione incluso si Time.timeScale es 0

        // 4. Ocultar el panel
        introPanel.SetActive(false);
        Debug.Log("Ocultando panel de introducción.");

        // 5. Reanudar el juego si se pausó
        if (pauseGameWhileDisplaying)
        {
            Time.timeScale = 1f; // Reanuda el juego
        }
        
        // Aquí podrías desencadenar otros eventos, como empezar el temporizador de la partida
        // GameManager.Instance.StartGameTimer();
    }

    // Método para detener la coroutine si la escena se cierra o el objeto se desactiva
    private void OnDisable()
    {
        // Asegurarse de reanudar el juego si se desactiva el panel mientras está pausado
        if (pauseGameWhileDisplaying && Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
        StopAllCoroutines(); // Detener coroutines al deshabilitar el script
    }
}