using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnObjects : MonoBehaviour
{
    [SerializeField] private SetList setList;
    [SerializeField] private GameObject[] objectPlaces;
    private List<Vector2> objectsInRange = new List<Vector2>();
    [SerializeField] private List<Object> objectsToSpawn = new List<Object>();
    public int maxObjects = 0;
    [SerializeField] private GameObject objectPrefab;
    
    // Lista pública de objetos spawneados con sus datos
    [Header("Spawned Objects Info")]
    public List<GameObject> spawnedObjects = new List<GameObject>();
    public List<Object> selectedObjectsData = new List<Object>();

    void Start()
    {
        foreach (GameObject place in objectPlaces)
        {
            objectsInRange.Add(place.transform.position);
        }

        Spawn();
    }

    void Spawn()
    {
        // Verificar que hay datos para spawnear
        if (objectsToSpawn.Count == 0 || objectsInRange.Count == 0 || objectPrefab == null)
        {
            Debug.LogWarning("SpawnObjects: Missing data for spawning!");
            return;
        }

        // Limpiar listas
        spawnedObjects.Clear();
        selectedObjectsData.Clear();

        // Crear una copia de las posiciones disponibles para evitar repetir posiciones
        List<Vector2> availablePositions = new List<Vector2>(objectsInRange);

        // Limitar maxObjects al número de posiciones disponibles
        int objectsToCreate = Mathf.Min(maxObjects, availablePositions.Count);

        for (int i = 0; i < objectsToCreate; i++)
        {
            // Elegir posición aleatoria
            int randomPositionIndex = Random.Range(0, availablePositions.Count);
            Vector2 spawnPosition = availablePositions[randomPositionIndex];
            availablePositions.RemoveAt(randomPositionIndex);

            // Elegir objeto aleatorio
            int randomObjectIndex = Random.Range(0, objectsToSpawn.Count);
            Object randomObjectData = objectsToSpawn[randomObjectIndex];

            // Instanciar el prefab
            GameObject spawnedObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

            // Agregar a las listas públicas
            spawnedObjects.Add(spawnedObject);
            selectedObjectsData.Add(randomObjectData);

            // Asignar el sprite del ScriptableObject al prefab
            SpriteRenderer spriteRenderer = spawnedObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && randomObjectData.Asset != null)
            {
                spriteRenderer.sprite = randomObjectData.Asset;
            }

            // Opcional: Guardar referencia al ScriptableObject en el GameObject
            ObjectData objectDataComponent = spawnedObject.GetComponent<ObjectData>();
            if (objectDataComponent != null)
            {
                objectDataComponent.SetObjectData(randomObjectData);
            }

            Debug.Log($"Spawned {randomObjectData.name} at position {spawnPosition}");
        }

        Debug.Log($"Total objects spawned: {spawnedObjects.Count}");
        setList.CreateList(spawnedObjects, selectedObjectsData);
    }

    // Método público para obtener info de un objeto específico
    public Object GetObjectData(GameObject obj)
    {
        int index = spawnedObjects.IndexOf(obj);
        return index >= 0 ? selectedObjectsData[index] : null;
    }
}
