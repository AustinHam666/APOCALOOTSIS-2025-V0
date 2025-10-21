using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Objects/Object")]
public class Object : ScriptableObject
{
    [Header("Object Properties")]
    [SerializeField] private Sprite asset;
    [SerializeField] private int valor;
    [SerializeField] private string objectName;


    public Sprite Asset => asset;
    public int Valor => valor;
    public string ObjectName => objectName;
}
