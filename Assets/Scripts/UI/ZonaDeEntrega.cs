using UnityEngine;
using TMPro; // Necesario para el texto
using System.Collections.Generic; // Necesario para las listas

public class ZonaDeEntrega : MonoBehaviour
{
    // 1. Arrastra tu UI de texto aquí en el Inspector
    public TextMeshProUGUI scoreText;

    // 2. La lista de nombres de objetos que buscamos
    private HashSet<string> targetNames = new HashSet<string>
    {
        "Objeto",
        "Objeto2",
        "Objeto3",
        "Objeto4",
        "Objeto5"
    };

    // 3. La lista de objetos que SÍ están en la zona
    private HashSet<string> objectsInZone = new HashSet<string>();

    void Start()
    {
        UpdateScore(); // Pone el contador en "0 / 5" al empezar
    }

    // Esta función la llamará el JUGADOR cuando SUELTE un objeto
    public void CheckObject(GameObject obj)
    {
        string objectName = obj.name;

        // Si el objeto tiene el nombre correcto Y no está ya en la lista
        if (targetNames.Contains(objectName) && !objectsInZone.Contains(objectName))
        {
            objectsInZone.Add(objectName);
            UpdateScore();
        }
    }

    // Esta función la llamará el JUGADOR cuando AGARRE un objeto
    public void RemoveObject(GameObject obj)
    {
        string objectName = obj.name;

        // Si el objeto está en nuestra lista de "entregados"
        if (objectsInZone.Contains(objectName))
        {
            objectsInZone.Remove(objectName);
            UpdateScore();
        }
    }

    // Actualiza el texto de la UI
    private void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = objectsInZone.Count.ToString() + " / 5";
        }
    }
}