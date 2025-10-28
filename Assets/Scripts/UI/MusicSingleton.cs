using UnityEngine;

public class MusicaSingleton : MonoBehaviour
{
    // Ahora la instancia es pública para que otros scripts la encuentren
    public static MusicaSingleton instance;

    // Guardamos la referencia al AudioSource
    private AudioSource miAudioSource;

    void Awake()
    {
        // Revisamos si ya existe una instancia
        if (instance == null)
        {
            // Si no existe, esta se convierte en la instancia
            instance = this;
            
            // Le dice a Unity que no destruya este GameObject
            DontDestroyOnLoad(gameObject);
            
            // Obtenemos el componente AudioSource
            miAudioSource = GetComponent<AudioSource>();
        }
        else
        {
            // Si ya existe una instancia (duplicado), se destruye
            Destroy(gameObject);
        }
    }

    // Nueva función pública para controlar la música desde fuera
    public void PausarMusica(bool pausar)
    {
        if (pausar)
        {
            miAudioSource.Pause();
        }
        else
        {
            miAudioSource.UnPause();
        }
    }
}