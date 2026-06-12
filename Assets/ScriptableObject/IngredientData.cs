using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "Scriptable Objects/IngredientData")]
public class IngredientData : ScriptableObject
{
    public GameObject Prefab;
    public IngredientID ID;
    public string Name;
}
