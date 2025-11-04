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
    public int player1Count = 0;
    public int player2Count = 0;
    public bool coopGame = true;
    [SerializeField] private GameObject endScreenUI;
    [SerializeField] private TMP_Text player1;
    [SerializeField] private TMP_Text player2;
    [SerializeField] private TMP_Text whoWins;
    int totalCount = 0;

    void Awake()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
        }
        
        // Inicializamos la puntuación estática
        aciertosFinales = 0;
        ActualizarUI();
    }

    // Esta función se activa AUTOMÁTICAMENTE cuando un collider se activa dentro de la zona
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player 1")
        {
            if (other.GetComponent<PlayerController>().isHolding)
            {
                GameObject objetoSostenido = other.GetComponent<PlayerController>().heldObject;
                if (objetoSostenido != null && !objetosEnZona.Contains(objetoSostenido))
                {
                    objetosEnZona.Add(objetoSostenido);
                    other.GetComponent<PlayerController>().DropObject();
                    player1Count++;
                    objetoSostenido.SetActive(false);
                }
            }
        }

        // CAMBIO AQUÍ: Player 2 en lugar de Player 1
        if (other.gameObject.name == "Player 2")
        {
            if (other.GetComponent<PlayerController>().isHolding)
            {
                GameObject objetoSostenido = other.GetComponent<PlayerController>().heldObject;
                if (objetoSostenido != null && !objetosEnZona.Contains(objetoSostenido))
                {
                    objetosEnZona.Add(objetoSostenido);
                    other.GetComponent<PlayerController>().DropObject();
                    player2Count++; // Ya está correcto
                    objetoSostenido.SetActive(false);
                }
            }
        }

        if (coopGame)
        {
            totalCount = player1Count + player2Count;
            if (totalCount >= capacidadMaxima)
            {
                Debug.Log("Capacidad máxima alcanzada en modo cooperativo.");
                gameObject.GetComponent<Collider2D>().enabled = false;
                //Logica para terminar el juego
            }
        } else {
            // logica de win de juego versus
        }
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
    
    public void SendEndScreen()
    {
        if (!coopGame)
        {
            player1.text = player1Count.ToString();
            player2.text = player2Count.ToString();


            if (player1Count > player2Count)
            {
                whoWins.text = "Jugador 1 Gana!";
            }
            else if (player2Count > player1Count)
            {
                whoWins.text = "Jugador 2 Gana!";
            }
            else
            {
                whoWins.text = "Empate!";
            }

            endScreenUI.SetActive(true);
        } else {
            player1.text = totalCount.ToString();
            endScreenUI.SetActive(true);
        }
    }
}