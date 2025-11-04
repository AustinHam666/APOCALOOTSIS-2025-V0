using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Users;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction sprintAction;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator playerAnimator;
    [SerializeField] public float moveSpeed = 5f;
    private List<GameObject> interactablesInRange = new List<GameObject>();
    
    // Variables para el sistema de attach
    public GameObject attachedObject;
    private Vector3 attachOffset;
    private bool isInteracting = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Prefer using a PlayerInput component (safer and avoids depending on a generated C# class)
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null && playerInput.actions != null)
        {
            // Try to get actions by name. Ensure your Input Actions asset has these action names.
            if (playerInput.actions.FindAction("Move") != null)
            {
                moveAction = playerInput.actions["Move"];
                moveAction.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
                moveAction.canceled += _ => moveInput = Vector2.zero;
            }

            if (playerInput.actions.FindAction("Interact") != null)
            {
                interactAction = playerInput.actions["Interact"];
                interactAction.started += _ => StartInteract();
                //interactAction.canceled += _ => StopInteract();
            }

            if (playerInput.actions.FindAction("Sprint") != null)
            {
                sprintAction = playerInput.actions["Sprint"];
                sprintAction.started += _ => moveSpeed = 8f;
                sprintAction.canceled += _ => moveSpeed = 5f;
            }
        }
        else
        {
            Debug.LogWarning("PlayerInput component or actions asset not found on this GameObject.\n" +
                             "Either add a PlayerInput with the expected actions (Move/Interact/Sprint)\n" +
                             "or re-generate the C# class for your .inputactions and update the script accordingly.");
        }
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        interactAction?.Enable();
        sprintAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        interactAction?.Disable();
        sprintAction?.Disable();
        //StopInteract();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void Update()
    {
        if (isInteracting && attachedObject != null)
        {
            attachedObject.transform.position = transform.position + attachOffset;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            interactablesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            interactablesInRange.Remove(other.gameObject);
            
            if (other.gameObject == attachedObject)
            {
                //StopInteract();
            }
        }
    }

    private void StartInteract()
    {
        if (interactablesInRange.Count > 0 && !isInteracting)
        {
            Debug.Log("Intentando interactuar con un objeto...");
            GameObject targetObject = interactablesInRange[0];

            Vector2 direction = (targetObject.transform.position - transform.position).normalized;
            string directionText = GetDirectionText(direction);

            //Debug.Log($"Started interacting with: {targetObject.GetComponent<ObjectData>()?.GetObjectData().ObjectName} - Direction: {directionText}");

            attachedObject = targetObject;
            attachOffset = targetObject.transform.position - transform.position;
            isInteracting = true;

            Rigidbody2D objectRb = attachedObject.GetComponent<Rigidbody2D>();
            if (objectRb != null)
            {
                // CAMBIO AQUÍ
                objectRb.bodyType = RigidbodyType2D.Kinematic;
            }
        } else if (isInteracting)
        {
            Debug.Log("Ya está interactuando con un objeto.");
        }
    }

    /*private void StopInteract()
    {
        if (isInteracting && attachedObject != null)
        {
            Debug.Log($"Stopped interacting with: {attachedObject.GetComponent<ObjectData>()?.GetObjectData().ObjectName}");
            
            Rigidbody2D objectRb = attachedObject.GetComponent<Rigidbody2D>();
            if (objectRb != null)
            {
                // CAMBIO AQUÍ
                objectRb.bodyType = RigidbodyType2D.Dynamic;
            }

            Debug.Log("Soltó el objeto");
            attachedObject = null;
            attachOffset = gameObject.transform.position + new Vector3(0, -10f);
            isInteracting = false;
        }
    }*/

    private string GetDirectionText(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? "Derecha" : "Izquierda";
        }
        else
        {
            return direction.y > 0 ? "Arriba" : "Abajo";
        }
    }
}