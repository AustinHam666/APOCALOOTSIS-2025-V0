using System.Collections;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float detectDistance = 1f;     // distancia del raycast
    public LayerMask interactableLayer;     // capa de objetos interactuables
    public KeyCode interactKey = KeyCode.E; // tecla de interacción (se puede override en el inspector)
    public int playerId = 1; // 1 = jugador 1, 2 = jugador 2
    public bool autoAssignKey = true; // si es true se asigna E para player 1 y P para player 2 en Start()

    private InteractableObject currentObject;
    private InteractableObject heldObject;
    private bool isChecking = false;
    private bool canHold = true;

    private void Start()
    {
        // Asignación automática de la tecla de interacción según el id de jugador
        if (autoAssignKey)
        {
            if (playerId == 1) interactKey = KeyCode.E;
            else if (playerId == 2) interactKey = KeyCode.P;
        }
        // Iniciar la detección continua
        StartCoroutine(DetectionLoop());
    }

    private IEnumerator DetectionLoop()
    {
        isChecking = true;

        while (isChecking)
        {
            // Dirección en la que mira el jugador (puede venir de animación o input)
            Vector2 direction = transform.right; // asume que "derecha" es adelante
            // Si no se configuró interactableLayer en el inspector (valor 0), usar las capas por defecto
            int layerMask = (interactableLayer.value == 0) ? Physics2D.DefaultRaycastLayers : interactableLayer.value;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectDistance, layerMask);

            if (hit.collider != null)
            {
                // intentar obtener componente InteractableObject
                currentObject = hit.collider.GetComponent<InteractableObject>();
                if (currentObject == null)
                {
                    Debug.Log("Hit collider '" + hit.collider.name + "' but it has no InteractableObject component.");
                }
                Debug.DrawRay(transform.position, direction * detectDistance, Color.green);
            }
            else
            {
                currentObject = null;
                Debug.DrawRay(transform.position, direction * detectDistance, Color.red);
            }

            yield return new WaitForSeconds(0.1f); // controla la frecuencia del raycast
        }
    }

    private void Update()
    {
        // Presiona E para agarrar/soltar:
        // - Si ya sostienes un objeto (heldObject != null) -> suéltalo
        // - Si no sostienes nada -> intenta agarrar el objeto detectado por el raycast (currentObject)
        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log($"PlayerInteraction (playerId={playerId}) - Interact key pressed: {interactKey}");
            Debug.Log($"currentObject = {(currentObject != null ? currentObject.name : "null")}");
            // Si ya tenemos un objeto sostenido, lo soltamos
            if (heldObject != null)
            {
                heldObject.Interact(transform);
                heldObject = null;
                canHold = true;
                return;
            }

            // Si no sostenemos nada, intentamos agarrar el objeto detectado
            if (currentObject != null && canHold && !currentObject.IsHeld)
            {
                // calcular posición de sujeción: delante del jugador en su dirección de mirada,
                // y un poco abajo para que quede natural (ej: -0.3 en Y local)
                Vector3 faceDir = transform.right.normalized; // dirección en la que mira
                float holdDistance = 0.8f; // distancia frontal donde se posiciona el objeto
                Vector3 holdWorldPos = transform.position + (Vector3)faceDir * holdDistance + new Vector3(0, -0.3f, 0);

                // Llamamos al overload que posiciona en world pos
                currentObject.Interact(transform, holdWorldPos);

                // después de interactuar, si el objeto ahora está sostenido, lo registramos
                if (currentObject.IsHeld)
                {
                    heldObject = currentObject;
                    canHold = false;
                }
            }
        }
    }
}
