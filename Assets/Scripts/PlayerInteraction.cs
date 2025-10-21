using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    // --- Variables que ya tenías ---
    public Transform holdParent; 
    private GameObject heldObject;     
    private Rigidbody2D heldObjectRB;
    private bool isHolding = false;
    private GameObject pickableObject; 

    // --- ¡NUEVA VARIABLE! ---
    // Guardará la zona en la que estamos parados
    private ZonaDeEntrega currentZone; 

    
    void Update()
    {
        if (isHolding && heldObject != null)
        {
            MoveHeldObject();
        }
    }

    // --- ¡NUEVAS FUNCIONES DE TRIGGER! ---
    // Detectan si entramos o salimos de la zona de entrega
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Si entramos en un trigger (nuestro o del objeto)
        if (other.CompareTag("Pickable"))
        {
            pickableObject = other.gameObject;
        }
        
        // ¡NUEVO! Comprueba si entramos en la zona de entrega
        if (other.CompareTag("DeliveryZone"))
        {
            currentZone = other.GetComponent<ZonaDeEntrega>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == pickableObject)
        {
            pickableObject = null;
        }

        // ¡NUEVO! Comprueba si salimos de la zona de entrega
        if (other.CompareTag("DeliveryZone"))
        {
            currentZone = null;
        }
    }

    
    // --- FUNCIÓN DE INTERACCIÓN ---
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!context.performed) return; 

        if (isHolding)
        {
            DropObject(); // Soltamos
        }
        else if (pickableObject != null) 
        {
            TryPickup(pickableObject); // Agarramos
        }
    }

    // --- FUNCIÓN DE AGARRAR (MODIFICADA) ---
    void TryPickup(GameObject objectToPickUp)
    {
        isHolding = true;
        heldObject = objectToPickUp; 
        heldObjectRB = heldObject.GetComponent<Rigidbody2D>();

        if (heldObjectRB != null)
        {
            heldObjectRB.isKinematic = true;
            heldObjectRB.linearVelocity = Vector2.zero;
        }

        heldObject.transform.parent = holdParent;
        heldObject.transform.localPosition = Vector3.zero;

        pickableObject = null;

        // --- ¡LÍNEA AÑADIDA! ---
        // Avisa a la zona (si estamos en una) que hemos AGARRADO un objeto
        if (currentZone != null)
        {
            currentZone.RemoveObject(heldObject);
        }
    }

    // --- FUNCIÓN DE SOLTAR (MODIFICADA) ---
    void DropObject()
    {
        if (heldObject == null) return;

        if (heldObjectRB != null)
        {
            heldObjectRB.isKinematic = false;
        }

        heldObject.transform.parent = null;
        pickableObject = heldObject; // Lo podemos volver a agarrar
        
        // --- ¡LÍNEA AÑADIDA! ---
        // Avisa a la zona (si estamos en una) que hemos SOLTADO un objeto
        if (currentZone != null)
        {
            currentZone.CheckObject(heldObject);
        }

        heldObject = null;
        heldObjectRB = null;
        isHolding = false;
    }

    // (MoveHeldObject no cambia)
    void MoveHeldObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.position = holdParent.position;
        }
    }
}