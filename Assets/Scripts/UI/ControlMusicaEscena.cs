using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ControlMusicaEscena : MonoBehaviour
{
    // --- ¡NUEVO! Variables del Playlist ---
    [Tooltip("La lista de canciones que sonarán en esta escena")]
    public AudioClip[] listaMusicaEscena;

    [Tooltip("Cuántos segundos sonará cada canción antes de cambiar")]
    public float tiempoPorCancion = 120f; // 2 minutos por defecto

    private AudioSource miAudioSource;
    private int indiceMusicaActual = 0;
    private float temporizador;

    void Awake()
    {
        // Preparamos el AudioSource de esta escena
        miAudioSource = GetComponent<AudioSource>();
        
        // --- ¡NUEVO! Asegurarse de que el AudioSource no haga loop ---
        miAudioSource.loop = false; // El script controlará la siguiente canción
        miAudioSource.playOnAwake = false; // El script la iniciará
    }

    void Start()
    {
        // 1. Pausar la música del menú (igual que antes)
        if (MusicaSingleton.instance != null)
        {
            MusicaSingleton.instance.PausarMusica(true);
        }
        
        // 2. ¡NUEVO! Iniciar nuestro propio playlist
        if (listaMusicaEscena.Length > 0)
        {
            miAudioSource.clip = listaMusicaEscena[0];
            miAudioSource.Play();
            temporizador = tiempoPorCancion; // Iniciar temporizador
        }
    }

    void Update()
    {
        // Si no hay canciones, no hacemos nada
        if (listaMusicaEscena.Length == 0)
        {
            return;
        }

        // --- ¡NUEVO! Lógica del Temporizador ---
        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            SiguienteCancion();
        }
    }

    void SiguienteCancion()
    {
        indiceMusicaActual++;
        if (indiceMusicaActual >= listaMusicaEscena.Length)
        {
            indiceMusicaActual = 0; // Volver al inicio de la lista
        }

        miAudioSource.clip = listaMusicaEscena[indiceMusicaActual];
        miAudioSource.Play();
        temporizador = tiempoPorCancion; // Reiniciar temporizador
    }

    void OnDestroy()
    {
        // 3. Reanudar la música del menú (igual que antes)
        if (MusicaSingleton.instance != null)
        {
            MusicaSingleton.instance.PausarMusica(false);
        }
    }
}