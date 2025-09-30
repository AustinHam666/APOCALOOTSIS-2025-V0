using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ajustes")]
    public float moveSpeed = 5f;

    [Header("Controles (Inspector)")]
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("PlayerController necesita un Rigidbody2D en " + gameObject.name);
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
        Vector2 newPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}
