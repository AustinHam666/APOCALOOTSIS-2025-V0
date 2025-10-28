using UnityEngine;
using UnityEngine.UI; 
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Configuración del Tiempo")]
    public float tiempoTotal = 60f;
    private float tiempoInicio;
    private float tiempoTranscurrido; // Se declara aquí para que sea accesible

    [Header("Referencias de UI")]
    public Canvas canvasPuntuacionJuego; // Canvas con el contador "0/5"
    public Image panelOscuridad;         // Panel Image que se vuelve negro
    public GameObject panelResultadoFinal; // GameObject que contiene la imagen de corazones y el texto

    private bool juegoTerminado = false;

    void Start()
    {
        // Aseguramos que el panel de resultado esté oculto al inicio
        if (panelResultadoFinal != null)
        {
            panelResultadoFinal.SetActive(false);
        }

        // El Canvas de puntuación del juego queda visible por defecto

        // Asegurarse de que el panel de oscuridad esté transparente
        if (panelOscuridad != null)
        {
            panelOscuridad.color = new Color(0, 0, 0, 0);
        }

        // Empezar el temporizador
        tiempoInicio = Time.time;
    }

    void Update()
    {
        if (juegoTerminado) return;

        // Calcula el tiempo transcurrido (ya declarada arriba)
        tiempoTranscurrido = Time.time - tiempoInicio;

        // --- LÓGICA DE OSCURIDAD ---
        float t = tiempoTranscurrido / tiempoTotal;
        t = Mathf.Clamp01(t); 

        if (panelOscuridad != null)
        {
            panelOscuridad.color = new Color(0, 0, 0, t);
        }

        // 5. Verificar si el tiempo se acabó
        if (tiempoTranscurrido >= tiempoTotal)
        {
            TerminarPartida();
        }
    }

    void TerminarPartida()
    {
        juegoTerminado = true;
        
        // 1. Asegurarse de que esté 100% oscuro
        if (panelOscuridad != null)
        {
            panelOscuridad.color = new Color(0, 0, 0, 1f);
        }

        // 2. Ocultar el contador de puntuación del juego ("0/5")
        if (canvasPuntuacionJuego != null) 
        {
            canvasPuntuacionJuego.gameObject.SetActive(false); 
        }

        // 3. Mostrar el PanelResultadoFinal completo (con corazones y texto)
        if (panelResultadoFinal != null)
        {
            panelResultadoFinal.SetActive(true);
            
            // 4. Actualizar el texto del resultado (usando la variable estática)
            // Esto asume que el TextMeshPro está como hijo del PanelResultadoFinal
            TextMeshProUGUI textoFinal = panelResultadoFinal.GetComponentInChildren<TextMeshProUGUI>();
            if (textoFinal != null)
            {
                 // Usamos la variable estática de ZonaDeEntrega
                 textoFinal.text = "Recolectaron " + ZonaDeEntrega.aciertosFinales + "/5 Objetos";
            }
        }
        
        // 5. Pausar el juego (detiene el movimiento y la música de juego)
        Time.timeScale = 0f;
    }
}