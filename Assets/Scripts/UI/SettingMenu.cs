using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer; // Lo configuramos en el inspector
    public Slider volumeSlider;
    public Button backToMenuButton; // Asigna el botón desde el inspector

    void Start()
    {
        // Recuperar valor guardado (si existe)
        if (PlayerPrefs.HasKey("volume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("volume");
            volumeSlider.value = savedVolume;
            audioMixer.SetFloat("volume", savedVolume);
        }

        // Ocultar el botón si estamos en el MainMenu
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            backToMenuButton.gameObject.SetActive(false);
        }
        else
        {
            backToMenuButton.gameObject.SetActive(true);
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume); // Guardar preferencia
    }

    public void BackToMenu()
    {
        PlayerPrefs.SetInt("hasStarted", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }
}
