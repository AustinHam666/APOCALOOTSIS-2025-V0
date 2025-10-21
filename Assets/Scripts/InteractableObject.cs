using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private bool isHeld = false;
    private Transform holder;
    private Rigidbody2D rb;
    private Vector3 holdLocalPos;
    private Vector3 lastHolderPosition;
    private Quaternion lastHolderRotation;
    private Collider2D col;
    private bool prevIsTrigger = false;

    // Estado público de solo lectura: indica si el objeto está actualmente sostenido
    // Utilizar desde el script del jugador para evitar intentar volver a agarrar algo
    public bool IsHeld { get { return isHeld; } }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        // Si existe Rigidbody2D, lo dejamos en kinematic para que la física no lo mueva
        // hasta que el jugador explícitamente lo recoja. Esto asegura que el objeto
        // no sea afectado por gravedad/vientos/collisions mientras esté en escena.
        if (rb != null)
        {
            // CAMBIO AQUÍ
            rb.bodyType = RigidbodyType2D.Kinematic; 
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Update()
    {
        // Si el objeto está sostenido, forzamos su posición local para que solo se mueva
        // cuando el jugador se mueva (evita movimiento independiente por rounding/physics)
        if (isHeld && holder != null)
        {
            // Solo actualizar la posición del objeto si el holder (jugador) se ha movido
            if (holder.position != lastHolderPosition || holder.rotation != lastHolderRotation)
            {
                // calcular posición mundial desde el offset local y aplicarla
                transform.position = holder.TransformPoint(holdLocalPos);
                lastHolderPosition = holder.position;
                lastHolderRotation = holder.rotation;
            }
        }
    }

    // Interact: toggle pick/drop. Version original: mantiene compatibilidad.
    public void Interact(Transform player)
    {
        if (!isHeld)
        {
            // Agarrar
            isHeld = true;
            holder = player;
            if (rb != null)
            {
                // seguir siendo kinematic mientras se sostiene (no queremos que la física empuje al objeto)
                rb.linearVelocity = Vector2.zero;
                
                // CAMBIO AQUÍ
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            // evitar parentear; en su lugar calculamos la posición mundial desde un offset local
            // y desactivamos las colisiones físicas con el jugador (trigger) mientras se sostiene
            if (col != null)
            {
                prevIsTrigger = col.isTrigger;
                col.isTrigger = true;
            }
            holdLocalPos = new Vector3(0, 1f, 0);
            transform.position = holder.TransformPoint(holdLocalPos);
            // inicializar último estado del holder para evitar que se aplique antes de cualquier movimiento
            lastHolderPosition = holder.position;
            lastHolderRotation = holder.rotation;
        }
        else
        {
            // Soltar
            isHeld = false;
            holder = null;
            // dejamos sin parent
            // no parented: transform.parent = null
            transform.SetParent(null);
            if (rb != null)
            {
                // Mantener kinematic para que el objeto no se mueva por física al soltarse.
                
                // CAMBIO AQUÍ
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }
            if (col != null)
            {
                col.isTrigger = prevIsTrigger;
            }
        }
    }

    // Overload: permite pasar una posición en mundo donde debe colocarse el objeto al agarrarlo.
    // Esto se usa para posicionar el objeto en la dirección hacia la que mira el jugador.
    public void Interact(Transform player, Vector3 holdWorldPosition)
    {
        if (!isHeld)
        {
            isHeld = true;
            holder = player;
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                
                // CAMBIO AQUÍ
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            // evitar parentear; desactivar colisiones físicas con el jugador mientras se sostiene
            if (col != null)
            {
                prevIsTrigger = col.isTrigger;
                col.isTrigger = true;
            }
            // Convertir la posición de mundo deseada a local respecto al player y asignarla
            transform.position = holdWorldPosition;
            // almacenar la posición local deseada para mantenerla fija durante el hold
            holdLocalPos = holder.InverseTransformPoint(holdWorldPosition);
            // inicializar último estado del holder
            lastHolderPosition = holder.position;
            lastHolderRotation = holder.rotation;
        }
        else
        {
            // Soltar: mantener en la posición actual y seguir siendo kinematic
            isHeld = false;
            holder = null;
            if (rb != null)
            {
                // CAMBIO AQUÍ
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.linearVelocity = Vector2.zero;
            }
            if (col != null)
            {
                col.isTrigger = prevIsTrigger;
            }
        }
    }
}