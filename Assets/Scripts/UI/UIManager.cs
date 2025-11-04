using UnityEngine;
using UnityEngine.UI; 
using System.Collections.Generic; 
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    public float volumeLevel = 1;
    public float volumeStep = 0.1f;

    public void DecreaseVolume()
    {
        volumeLevel -= volumeStep;
        SetAudioVolume();
        if (volumeLevel <= 0) return;
    }

    public void IncreaseVolume()
    {
        volumeLevel += volumeStep;
        SetAudioVolume();
        if (volumeLevel >= 1) return;
    }

    void SetAudioVolume()
    {
        volumeLevel = Mathf.Clamp01(volumeLevel); // Fuerza entre 0 y 1
        AudioListener.volume = volumeLevel;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}