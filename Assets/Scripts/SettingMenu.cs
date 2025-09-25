using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer; // Lo configuramos en el inspector
    public Slider volumeSlider;

    void Start()
    {
        // Recuperar valor guardado (si existe)
        if (PlayerPrefs.HasKey("volume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = savedVolume;
            audioMixer.SetFloat("volume", savedVolume);
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume); // Guardar preferencia
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
