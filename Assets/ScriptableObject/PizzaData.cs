using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PizzaData", menuName = "Scriptable Objects/PizzaData")]
public class PizzaData : ScriptableObject
{
    public PizzaType type;
    public ObjectData objectData;
    public List<IngredientID> ingredients=new();
}
