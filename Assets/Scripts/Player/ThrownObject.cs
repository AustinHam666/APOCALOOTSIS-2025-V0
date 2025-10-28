using UnityEngine;

public class ThrownObject : MonoBehaviour
{
    // El Rigidbody del objeto
    private Rigidbody2D rb;
    
    // El jugador que lanzó este objeto (para evitar golpearse a sí mismo)
    public GameObject owner; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Revisa si chocamos con el jugador que nos lanzó. Si es así, no hacemos nada.
        if (collision.gameObject == owner)
        {
            return;
        }

        // 2. Revisa si chocamos con CUALQUIER jugador (que tenga el script PlayerController)
        PlayerController hitPlayer = collision.gameObject.GetComponent<PlayerController>();
        
        if (hitPlayer != null)
        {
            // ¡Golpeamos a un jugador! Llamamos a su función pública para aturdirlo
            hitPlayer.GetHit();
        }

        // 3. Después de chocar con CUALQUIER cosa (un jugador, una pared, etc.)
        // nos detenemos y volvemos a ser un objeto normal "no empujable".
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // Vuelve a ser Kinematic
            rb.linearVelocity = Vector2.zero; // Detiene todo movimiento
        }

        // 4. Finalmente, destruimos este script para que no pueda golpear de nuevo.
        Destroy(this);
    }
}