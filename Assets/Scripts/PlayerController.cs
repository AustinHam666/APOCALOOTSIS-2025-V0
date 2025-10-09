using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ajustes")]
    // moveSpeed ha sido movido a PlayerMovement; este script leerá la velocidad actual desde allí.

    [Header("Controles (Inspector)")]
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    private Rigidbody2D rb;
    private Vector2 movement;
    private PlayerMovement playerMovement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("PlayerController necesita un Rigidbody2D en " + gameObject.name);
        // intentar obtener PlayerMovement para leer la velocidad actual
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // Leemos input en Update (porque Input.* es frame-based)
        movement = Vector2.zero;
        if (Input.GetKey(upKey)) movement.y += 1f;

        //DETECTAR QUE JOYSTICK ES EL QUE SE ESTA USANDO DEPENDIENDO EL PERSONAJE
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            Debug.Log("Arriba");
            movement.y += 1f;
        }
            
        if (Input.GetKey(downKey)) movement.y -= 1f;
        //if (Input.GetAxisRaw("Vertical") < 0) movement.y -= 1f;
        if (Input.GetKey(leftKey)) movement.x -= 1f;
        //if (Input.GetAxisRaw("Horizontal") < 0) movement.x -= 1f;
        if (Input.GetKey(rightKey)) movement.x += 1f;
        //if (Input.GetAxisRaw("Horizontal") > 0) movement.x += 1f;
        movement = movement.normalized; // evita mover más rápido en diagonal
    }

    void FixedUpdate()
    {
        // Movimiento físico consistente
        float speedToUse = (playerMovement != null) ? playerMovement.CurrentSpeed : 5f; // fallback 5
        Vector2 newPos = rb.position + movement * speedToUse * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}
