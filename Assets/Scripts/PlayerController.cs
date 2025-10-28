using UnityEngine;
using System.Collections; // ¡Importante! Necesario para las Corutinas

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Ajustes de Movimiento")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    
    [Header("Controles (Inspector)")]
    public KeyCode upKey;
    public KeyCode downKey;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public KeyCode sprintKey;
    public KeyCode interactKey; // Tecla para agarrar/soltar
    public KeyCode throwKey;    // ¡NUEVO! Tecla para arrojar (T o L)

    [Header("Lógica de Agarre")]
    public Transform holdParent; // El "holdPoint" hijo del jugador
    public float throwForce = 10f;  // ¡NUEVO! Fuerza del lanzamiento
    public float stunDuration = 0.5f; // ¡NUEVO! Duración del aturdimiento
    
    private GameObject heldObject;
    private Rigidbody2D heldObjectRB;
    private GameObject pickableObject;

    // Componentes
    private Rigidbody2D rb;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer; // ¡NUEVO! Para el flash rojo
    private Vector2 movement;
    
    // Estados
    private bool isHolding = false;
    private bool isSprinting = false;
    private bool canMove = true; // ¡NUEVO! Para el aturdimiento
    private Vector2 lastMoveDirection = new Vector2(0, -1); // ¡NUEVO! Para saber dónde arrojar

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // ¡NUEVO! Obtenemos el SpriteRenderer
        
        if (holdParent == null)
        {
            Debug.LogError("Asigna el 'Hold Parent' (holdPoint) en el Inspector.");
        }
    }

    void Update()
    {
        // Si no nos podemos mover (aturdidos), no procesamos input de movimiento
        if (!canMove)
        {
            movement = Vector2.zero;
        }
        else
        {
            // --- 1. DETECCIÓN DE INPUT ---
            movement = Vector2.zero;
            if (Input.GetKey(upKey)) movement.y += 1f;
            if (Input.GetKey(downKey)) movement.y -= 1f;
            if (Input.GetKey(leftKey)) movement.x -= 1f;
            if (Input.GetKey(rightKey)) movement.x += 1f;
            
            movement = movement.normalized; // Normalizar
            
            // Input de Sprint
            isSprinting = Input.GetKey(sprintKey);
        }

        // --- Input de Interacción (Agarrar/Soltar) ---
        // Solo podemos interactuar si no estamos aturdidos
        if (canMove && Input.GetKeyDown(interactKey))
        {
            if (isHolding)
            {
                DropObject();
            }
            else // Si no estás sosteniendo nada
            {
                playerAnimator.SetTrigger("Grab");
                if (pickableObject != null)
                {
                    PickUpObject();
                }
            }
        }

        // --- ¡NUEVO! Input de Arrojar ---
        // Solo podemos arrojar si estamos sosteniendo algo y no estamos aturdidos
        if (canMove && isHolding && Input.GetKeyDown(throwKey))
        {
            ThrowObject();
        }

        // --- 2. ACTUALIZAR ANIMATOR ---
        bool isMoving = movement.magnitude > 0.01f;
        
        playerAnimator.SetBool("IsMoving", isMoving);
        playerAnimator.SetBool("IsHolding", isHolding);
        playerAnimator.SetFloat("Speed", isSprinting ? 1f : 0f); // 0=walk, 1=run

        // Actualiza la dirección solo si nos movemos
        if (isMoving)
        {
            playerAnimator.SetFloat("MoveX", movement.x);
            playerAnimator.SetFloat("MoveY", movement.y);
            lastMoveDirection = movement; // ¡NUEVO! Guardamos la última dirección
        }
    }

    void FixedUpdate()
    {
        // --- 3. MOVIMIENTO FÍSICO ---
        // ¡NUEVO! Si no nos podemos mover, salimos de la función
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero; // Asegura que el jugador esté quieto
            return;
        }
        
        float currentSpeed = (isSprinting && !isHolding) ? runSpeed : walkSpeed;
        Vector2 newPos = rb.position + movement * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }
    
    // --- 4. LÓGICA DE DETECCIÓN (Triggers) ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            pickableObject = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == pickableObject)
        {
            pickableObject = null;
        }
    }

    // --- 5. LÓGICA DE AGARRAR / SOLTAR / ARROJAR ---
    void PickUpObject()
    {
        heldObject = pickableObject;
        heldObjectRB = heldObject.GetComponent<Rigidbody2D>();

        if (heldObjectRB != null)
        {
            heldObjectRB.bodyType = RigidbodyType2D.Kinematic;
        }

        Collider2D heldCollider = heldObject.GetComponent<Collider2D>();
        if (heldCollider != null)
        {
            heldCollider.enabled = false;
        }

        heldObject.transform.SetParent(holdParent);
        heldObject.transform.localPosition = Vector3.zero;
        isHolding = true;
    }

    void DropObject()
    {
        if (heldObject == null) return;

        Collider2D heldCollider = heldObject.GetComponent<Collider2D>();
        if (heldCollider != null)
        {
            heldCollider.enabled = true;
        }

        if (heldObjectRB != null)
        {
            heldObjectRB.bodyType = RigidbodyType2D.Kinematic; // Sigue siendo Kinematic (no empujable)
            heldObjectRB.linearVelocity = Vector2.zero; 
        }

        heldObject.transform.SetParent(null);
        heldObject = null;
        heldObjectRB = null;
        isHolding = false;
    }

    // --- ¡NUEVA FUNCIÓN DE ARROJAR! ---
    void ThrowObject()
    {
        if (heldObject == null) return;

        // 1. Reactivamos el collider
        Collider2D heldCollider = heldObject.GetComponent<Collider2D>();
        if (heldCollider != null)
        {
            heldCollider.enabled = true;
        }

        // 2. Lo soltamos
        heldObject.transform.SetParent(null);
        
        // 3. ¡Le añadimos el script de proyectil!
        ThrownObject projectile = heldObject.AddComponent<ThrownObject>();
        projectile.owner = this.gameObject; // Le decimos quién es el dueño

        // 4. Lo convertimos en objeto físico y le damos fuerza
        if (heldObjectRB != null)
        {
            heldObjectRB.bodyType = RigidbodyType2D.Dynamic; // ¡Ahora SÍ es Dinámico!
            heldObjectRB.linearVelocity = Vector2.zero; // Reseteamos velocidad
            heldObjectRB.AddForce(lastMoveDirection * throwForce, ForceMode2D.Impulse);
        }

        // 5. Reseteamos el estado del jugador
        heldObject = null;
        heldObjectRB = null;
        isHolding = false;
    }

    // --- ¡NUEVA FUNCIÓN PÚBLICA PARA SER GOLPEADO! ---
    public void GetHit()
    {
        // No podemos ser golpeados si ya estamos aturdidos
        if (canMove)
        {
            StartCoroutine(StunRoutine());
        }
    }

    // --- ¡NUEVA CORUTINA DE ATURDIMIENTO! ---
    private IEnumerator StunRoutine()
    {
        // 1. Aturdir y enrojecer
        canMove = false;
        spriteRenderer.color = Color.red;

        // 2. Esperar
        yield return new WaitForSeconds(stunDuration); // Espera 0.5 segundos

        // 3. Quitar aturdimiento y volver al color normal
        canMove = true;
        spriteRenderer.color = Color.white;
    }
}