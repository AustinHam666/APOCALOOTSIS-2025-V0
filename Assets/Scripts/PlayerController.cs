using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ajustes")]
    public float moveSpeed = 5f;

    [Header("Controles (Inspector)")]
    public KeyCode upKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;

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
        if (Input.GetKey(upKey))    movement.y += 1f;
        if (Input.GetKey(downKey))  movement.y -= 1f;
        if (Input.GetKey(leftKey))  movement.x -= 1f;
        if (Input.GetKey(rightKey)) movement.x += 1f;
        movement = movement.normalized; // evita mover más rápido en diagonal
    }

    void FixedUpdate()
    {
        // Movimiento físico consistente
        Vector2 newPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
}
