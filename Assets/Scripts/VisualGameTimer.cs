using UnityEngine;
using UnityEngine.UI; // Necesario si usas el componente Image (UI)
using System.Collections;
using Unity.VisualScripting; // Necesario para Coroutines

public class VisualGameTimer : MonoBehaviour
{
    [Header("Configuración del Temporizador")]
    [Tooltip("Duración total del temporizador en segundos.")]
    public float totalGameTime = 60f; // Por ejemplo, 60 segundos (1 minuto)

    [Header("Sprites del Temporizador")]
    [Tooltip("Lista de sprites para mostrar el progreso del tiempo, de lleno a vacío.")]
    public Sprite[] timerSprites; // Arrastra tus sprites aquí en el Inspector

    [Header("Componente de Visualización")]
    [Tooltip("El componente SpriteRenderer (para objetos 2D) o Image (para UI) que mostrará los sprites.")]
    public SpriteRenderer targetSpriteRenderer; // Asigna aquí si es un SpriteRenderer
    public Image targetUIImage; // Asigna aquí si es un Image (UI)

    private float currentTime;
    private int currentSpriteIndex = 0;
    private bool timerRunning = false;

    // --- PROPIEDADES PÚBLICAS (para que otros scripts puedan leerlas) ---
    public float CurrentTime { get { return currentTime; } }
    public bool IsTimerRunning { get { return timerRunning; } }

    [SerializeField] private ZonaDeEntrega zonaDeEntrega;

    void Start()
    {
        // Validación inicial
        if (timerSprites == null || timerSprites.Length == 0)
        {
            Debug.LogError("VisualGameTimer: No hay sprites asignados para el temporizador. Por favor, asigna sprites en el Inspector.");
            enabled = false; // Desactiva el script si no hay sprites
            return;
        }

        if (targetSpriteRenderer == null && targetUIImage == null)
        {
            Debug.LogError("VisualGameTimer: No se ha asignado un SpriteRenderer ni un Image para mostrar el temporizador. Asigna uno en el Inspector.");
            enabled = false;
            return;
        }
        
        // Empezamos con el tiempo máximo y el primer sprite (lleno)
        currentTime = totalGameTime;
        UpdateVisualTimer(); // Asegurarse de que el sprite inicial sea el correcto
        StartTimer(); // Inicia el temporizador automáticamente al empezar
    }

    // Método para iniciar el temporizador
    public void StartTimer()
    {
        if (!timerRunning)
        {
            timerRunning = true;
            StartCoroutine(Countdown());
        }
    }

    // Método para detener el temporizador
    public void StopTimer()
    {
        timerRunning = false;
        StopAllCoroutines(); // Detiene cualquier coroutine de temporizador en ejecución
    }

    // Coroutine para el conteo regresivo
    IEnumerator Countdown()
    {
        while (currentTime > 0)
        {
            yield return null; // Espera un frame
            if (!timerRunning) break; // Si se detiene el temporizador, sal del bucle

            currentTime -= Time.deltaTime; // Reduce el tiempo
            UpdateVisualTimer(); // Actualiza el sprite

            // Si el tiempo llega a cero, asegúrate de que se muestre el sprite de "vacío"
            if (currentTime <= 0)
            {
                currentTime = 0;
                UpdateVisualTimer();
                Debug.Log("¡Tiempo terminado!");
                zonaDeEntrega.SendEndScreen();
                StopTimer(); // Detiene el temporizador al finalizar
                // Aquí podrías llamar a un método para Game Over, fin de ronda, etc.
                // GameManager.Instance.EndRound(); 
            }
        }
    }

    // Actualiza el sprite basado en el tiempo actual
    void UpdateVisualTimer()
    {
        if (timerSprites == null || timerSprites.Length == 0) return;

        // Calcula el progreso del tiempo (de 0 a 1)
        float progress = currentTime / totalGameTime;

        // Calcula qué sprite debería mostrarse
        // Si hay N sprites, los índices van de 0 a N-1.
        // El último sprite (N-1) es para cuando el tiempo está vacío.
        // El primer sprite (0) es para cuando el tiempo está lleno.
        
        // Multiplicamos por Length - 1 para que el último sprite sea para 0 progreso.
        int newIndex = Mathf.FloorToInt(progress * (timerSprites.Length - 1));
        
        // Invertimos el índice para que 100% sea el primer sprite (lleno) y 0% el último (vacío)
        newIndex = (timerSprites.Length - 1) - newIndex;

        // Asegurarse de que el índice esté dentro de los límites
        newIndex = Mathf.Clamp(newIndex, 0, timerSprites.Length - 1);

        if (newIndex != currentSpriteIndex)
        {
            currentSpriteIndex = newIndex;
            if (targetSpriteRenderer != null)
            {
                targetSpriteRenderer.sprite = timerSprites[currentSpriteIndex];
            }
            else if (targetUIImage != null)
            {
                targetUIImage.sprite = timerSprites[currentSpriteIndex];
            }
        }
    }
}