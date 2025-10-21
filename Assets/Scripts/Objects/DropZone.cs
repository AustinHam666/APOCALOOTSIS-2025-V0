using UnityEngine;

public class DropZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"DETECTÓ ALGO: {other.name} con tag: {other.tag}");

        if (other.CompareTag("Interactable"))
        {
            Debug.Log($"--- COLISION CON: {other.name}");
        }
    }
}
