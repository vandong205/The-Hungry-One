using System;
using UnityEngine;

public class IngredientStray : MonoBehaviour,IInteractableObject
{
    public Action<IngredientID,Transform,GameObject> OnStrayPicked;
    [SerializeField] IngredientData data;
    private string id;
    public string ID { get => id; set => id=value; }
    public void Interact(string sender)
    {
        OnStrayPicked?.Invoke(data.ID,transform,data.Prefab);
    }
}
