using System.Collections.Generic;
using UnityEngine;
public enum PizzaType
{
    None,
    Margherita,
    Pepperoni,
    Hawaiian,
    Funghi
}
[CreateAssetMenu(fileName = "PizzaDatabase", menuName = "Scriptable Objects/PizzaDatabase")]
public class PizzaDatabase : ScriptableObject
{
    [SerializeField] List<PizzaData> pizzaDatas=new();
    private Dictionary<PizzaType,PizzaData> _lookup=new();
    private Dictionary<int,PizzaType> _recipes = new();
    private void BuildDict()
    {
        _lookup=new();
        foreach(PizzaData data in pizzaDatas)
        {
            _lookup.Add(data.type,data);
        }
    }
    private void BuildRecipeDict()
    {
        _recipes = new();

        foreach (PizzaData data in pizzaDatas)
        {
            if (data == null)
                continue;

            _recipes.Add(
                CreateMask(data.ingredients.ToArray()),
                data.type);
        }
    }
    public PizzaData GetPizzaData(PizzaType type)
    {
        if(_lookup.Count==0) BuildDict();
        if(_lookup.TryGetValue(type,out PizzaData data))
        {
            return data;
        }
        return null;
    }
    public PizzaType GetPizzaTypeByIngredients(params IngredientID[] ingredient)
    {
        if(_recipes.Count==0) BuildRecipeDict();
        int mask = CreateMask(ingredient);
        if(_recipes.TryGetValue(mask,out PizzaType type))
        {
            return type;
        }
        return PizzaType.None;
    }
    public int CreateMask(params IngredientID[] ingredients)
    {
        int mask = 0;
            foreach (var ingredient in ingredients)
            {
                mask |= 1 << (int)ingredient;
            }

        return mask;
    }

}
