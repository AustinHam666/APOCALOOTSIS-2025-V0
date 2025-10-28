using UnityEngine;
using TMPro; 
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class ZonaDeEntrega : MonoBehaviour
{
    // --- ¡SOLUCIÓN DE ERROR! Esta variable debe ser public static ---
    public static int aciertosFinales = 0; 

    [Header("Configuración de la Zona")]
    public int capacidadMaxima = 5;
    public TextMeshProUGUI textoAciertos; 
    public TextMeshProUGUI textoTotal; 	

    [Header("Referencias")]
    public UIManager uiManager;

    // La lista de nombres de objetos que SÍ son correctos
    private HashSet<string> nombresCorrectos = new HashSet<string>
    {
        "Objeto", "Objeto2", "Objeto3", "Objeto4", "Objeto5"
    };

    private List<GameObject> objetosEnZona = new List<GameObject>();

    void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
        }

        // Revisar si el UIManager fue asignado
        if (uiManager == null)
        {
            Debug.LogError("¡ERROR! Falta asignar el UIManager en la ZonaDeEntrega. Arrástralo al Inspector.");
        }
        
        // Inicializamos la puntuación estática
        aciertosFinales = 0;
        ActualizarUI();
    }

    // Esta función se activa AUTOMÁTICAMENTE cuando un collider se activa dentro de la zona
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignora al jugador, paredes, etc.
        if (!other.CompareTag("Interactable"))
        {
            return; 
        }

        // Si la zona está llena, no hagas nada
        if (objetosEnZona.Count >= capacidadMaxima)
        {
            return;
        }

        GameObject objetoEntregado = other.gameObject;

        // Si ya está en la lista, no hagas nada
        if (objetosEnZona.Contains(objetoEntregado))
        {
            return;
        }

        // --- ¡Objeto Añadido! ---
        objetosEnZona.Add(objetoEntregado);
        objetoEntregado.tag = "Untagged"; // Quita el tag para no volver a agarrarlo
        
        Rigidbody2D objRb = objetoEntregado.GetComponent<Rigidbody2D>();
        if (objRb != null)
        {
            objRb.bodyType = RigidbodyType2D.Kinematic;
        }

        // --- LÓGICA DE CORAZONES/ERROR ---
        // Revisamos si el nombre del objeto NO está en la lista de correctos
        if (!nombresCorrectos.Contains(objetoEntregado.name))
        {
            // ¡Es un error! Llamamos al UIManager para quitar un corazón
            if (uiManager != null)
            {
                uiManager.QuitarCorazon();
            }
            Debug.Log("¡ERROR! Objeto incorrecto entregado: " + objetoEntregado.name);
        }

        // Actualizar la UI y la puntuación estática
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        int aciertos = 0;
        
        foreach (GameObject obj in objetosEnZona)
        {
            if (nombresCorrectos.Contains(obj.name))
            {
                aciertos++;
            }
        }
        
        // --- ¡ACTUALIZA LA VARIABLE ESTÁTICA AQUÍ! ---
        aciertosFinales = aciertos; 

        if (textoAciertos != null)
        {
            textoAciertos.text = "Aciertos: " + aciertos + " / " + nombresCorrectos.Count;
        }

        if (textoTotal != null)
        {
            textoTotal.text = "Total en Zona: " + objetosEnZona.Count + " / " + capacidadMaxima;
        }
    }
}