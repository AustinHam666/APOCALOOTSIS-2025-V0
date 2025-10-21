using UnityEngine;
// using Unity.VisualScripting; // No es necesario si no usas Visual Scripting directamente en este script
// using UnityEngine.InputSystem; // No es necesario si usas el Input System antiguo

[RequireComponent(typeof(Rigidbody2D))] // Asegura que el GameObject tenga un Rigidbody2D
[RequireComponent(typeof(Animator))]    // Asegura que el GameObject tenga un Animator
public class PlayerController : MonoBehaviour
{
    [Header("Ajustes")]
    // moveSpeed ha sido movido a PlayerMovement; este script leerá la velocidad actual desde allí.
    // Necesitamos una velocidad para este script si no va a leerla de PlayerMovement para mover el RB
    public float moveSpeed = 5f; // Velocidad de movimiento predeterminada para este controlador si PlayerMovement no lo provee.

    [Header("Controles (Inspector)")]
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;

    private Rigidbody2D rb;
    private Vector2 movement; // Para el input de movimiento
    private PlayerMovement playerMovement; // Para obtener moveSpeed (si existe)

    // Declaración de las variables que faltaban y que daban error
    private Animator playerAnimator; // Declarado aquí

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("PlayerController necesita un Rigidbody2D en " + gameObject.name);
        
        // playerInput = GetComponent<PlayerInput>(); // Esta línea causaba error porque 'playerInput' no estaba declarada y no usamos el nuevo Input System aquí.
                                                    // Si quieres usar el nuevo Input System, esto debería ser parte de un PlayerMovement o un script de input.

       playerAnimator = GetComponentInChildren<Animator>(); // Obtener el Animator
        if (playerAnimator == null)
        {
            Debug.LogWarning("PlayerController: Animator component not found on this GameObject.", this);
        }

        // Intentar obtener PlayerMovement para leer la velocidad actual si está presente
        playerMovement = GetComponent<PlayerMovement>();
        // Si PlayerMovement no está presente o no se va a usar para la velocidad, 'moveSpeed' de este script será el predeterminado.
    }

    void Update()
    {
        // === Lógica de Lectura de Input (usando el Input System antiguo) ===
        // Esto es lo que ya tenías, lo mantendremos como está, pero ten en cuenta
        // que es diferente al nuevo Input System que usa PlayerMovement.

        movement = Vector2.zero;
        
        // Input por KeyCode asignable en Inspector
        if (Input.GetKey(upKey)) movement.y += 1f;
        if (Input.GetKey(downKey)) movement.y -= 1f;
        if (Input.GetKey(leftKey)) movement.x -= 1f;
        if (Input.GetKey(rightKey)) movement.x += 1f;

        // Input por GetAxisRaw (para joystick/teclas predefinidas de Unity)
        // OJO: Si usas estos al mismo tiempo que los KeyCode, se sumarán.
        // Si estás detectando qué joystick es, probablemente solo quieras una de estas fuentes de input.
        // Asegúrate de que tus ejes "Vertical" y "Horizontal" estén configurados en Project Settings -> Input Manager
        if (Input.GetAxisRaw("Vertical") > 0.01f) // Usa un pequeño umbral para evitar ruidos del joystick
        {
            // Debug.Log("Arriba (Axis)"); // Puedes descomentar para depurar
            movement.y += 1f;
        }
        if (Input.GetAxisRaw("Vertical") < -0.01f)
        {
            movement.y -= 1f;
        }
        if (Input.GetAxisRaw("Horizontal") < -0.01f)
        {
            movement.x -= 1f;
        }
        if (Input.GetAxisRaw("Horizontal") > 0.01f)
        {
            movement.x += 1f;
        }
        
        movement = movement.normalized; // evita mover más rápido en diagonal
    }

    void FixedUpdate()
    {
        // === Lógica de Movimiento Físico ===
        // Obtener la velocidad: primero intenta de PlayerMovement, si no, usa la propia de este script
        float speedToUse = (playerMovement != null) ? playerMovement.moveSpeed : this.moveSpeed;

        // Mueve el Rigidbody2D
        Vector2 newPos = rb.position + movement * speedToUse * Time.fixedDeltaTime;
        rb.MovePosition(newPos);

        // === Lógica de Animación ===
        if (playerAnimator != null)
        {
            // 'movement' ahora es el Vector2 que ya calculaste en Update()
            bool isMoving = movement.magnitude > 0.01f; // Detecta si hay movimiento significativo

            playerAnimator.SetFloat("MoveX", movement.x);
            playerAnimator.SetFloat("MoveY", movement.y);
            playerAnimator.SetBool("IsMoving", isMoving);

            // Si tienes un parámetro IsSprinting, y tienes lógica para detectarlo
            // playerAnimator.SetBool("IsSprinting", playerMovement != null && playerMovement.IsSprinting()); 
            // Esto requeriría que PlayerMovement tenga una propiedad o método IsSprinting
        }
    }
}