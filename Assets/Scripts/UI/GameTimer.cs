using UnityEngine;
using UnityEngine.UI; // Necesario para la imagen (Image)
using TMPro; // Si tu contador también es TextMeshPro

public class GameTimer : MonoBehaviour
{
    [Header("Configuración del Tiempo")]
    public float tiempoTotal = 50f;
    private float tiempoActual;

    [Header("Referencias de UI")]
    public Canvas canvasPuntuacion;    // Arrastra tu canvas "0 / 5" aquí
    public Image panelOscuridad;       // Arrastra tu "PanelOscuridad" aquí

    private bool juegoTerminado = false;

    void Start()
    {
        // 1. Ocultar el canvas de puntuación al inicio
        if (canvasPuntuacion != null)
        {
            canvasPuntuacion.gameObject.SetActive(false);
        }

        // 2. Asegurarse de que el panel de oscuridad esté transparente
        if (panelOscuridad != null)
        {
            panelOscuridad.color = new Color(0, 0, 0, 0);
        }

        // 3. Empezar el temporizador
        tiempoActual = tiempoTotal;
    }

    void Update()
    {
        // Si el juego ya terminó, no hagas nada
        if (juegoTerminado)
        {
            return;
        }

        // 4. Restar tiempo
        if (tiempoActual > 0)
        {
            tiempoActual -= Time.deltaTime;

            // --- LÓGICA DE OSCURIDAD ---
            // Calculamos cuánto tiempo ha pasado
            float tiempoTranscurrido = tiempoTotal - tiempoActual;

            // Calculamos el alfa (transparencia) objetivo.
            // Si han pasado 50s, el alfa será 1 (100% opaco).
            // Si han pasado 10s, el alfa será 10/50 = 0.2 (20% opaco).
            // Si han pasado 20s, el alfa será 20/50 = 0.4 (40% opaco).
            // Esto coincide exactamente con tus 10s.
            float targetAlpha = tiempoTranscurrido / tiempoTotal;

            if (panelOscuridad != null)
            {
                panelOscuridad.color = new Color(0, 0, 0, targetAlpha);
            }
        }
        else
        {
            // 5. El tiempo se acabó
            TerminarPartida();
        }
    }

    void TerminarPartida()
    {
        juegoTerminado = true;
        tiempoActual = 0;

        // 6. Asegurarse de que esté 100% oscuro
        if (panelOscuridad != null)
        {
            panelOscuridad.color = new Color(0, 0, 0, 1f);
        }

        // 7. Mostrar el canvas de puntuación final
        if (canvasPuntuacion != null)
        {
            canvasPuntuacion.gameObject.SetActive(true);
        }

        // 8. (Opcional) Pausar el juego
        Time.timeScale = 0f;
    }
}