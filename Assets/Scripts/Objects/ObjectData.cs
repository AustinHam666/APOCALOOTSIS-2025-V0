using UnityEngine;

public class ObjectData : MonoBehaviour
{
    private Object objectData;

    public Object GetObjectData() => objectData;

    public void SetObjectData(Object data)
    {
        objectData = data;
        
        // Actualizar el sprite automáticamente
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && data.Asset != null)
        {
            spriteRenderer.sprite = data.Asset;
        }
    }

    public int GetValue() => objectData?.Valor ?? 0;
}