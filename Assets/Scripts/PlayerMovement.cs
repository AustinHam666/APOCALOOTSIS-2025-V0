using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;
    public float sprintSpeed = 5f; // velocidad al sprintar
    public int playerId = 1; // 1 o 2
    public KeyCode sprintKey = KeyCode.R; // asignado en Start() si se usa playerId

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isSprinting = false;

    // Exponer la velocidad actual para otros scripts (p. ej. PlayerController)
    public float CurrentSpeed { get { return isSprinting ? sprintSpeed : speed; } }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // movimiento top-down 2D por defecto
        // asignación automática de la tecla de sprint según el id del jugador
        if (playerId == 1) sprintKey = KeyCode.R;
        else if (playerId == 2) sprintKey = KeyCode.O;
        Debug.Log($"PlayerMovement started for playerId={playerId}, sprintKey={sprintKey}, speed={speed}, sprintSpeed={sprintSpeed}");
    }

    void Update()
    {
        // Input de movimiento
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        // Detectar sprint (tecla sostenida)
        bool sprintNow = Input.GetKey(sprintKey);
        if (sprintNow != isSprinting)
        {
            isSprinting = sprintNow;
            Debug.Log($"Player {playerId} sprint state: {isSprinting}");
        }
    }

    void FixedUpdate()
    {
    float currentSpeed = isSprinting ? sprintSpeed : speed;

        Vector2 newPos = rb.position + movement * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        // Opcional: rotar al jugador hacia la dirección de movimiento si hay alguna
        if (movement != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
