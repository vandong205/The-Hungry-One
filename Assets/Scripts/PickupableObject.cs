using System;
using UnityEngine;

public class PickupableObject : MonoBehaviour, IInteractableObject
{
   [SerializeField] string id;
    public string  ID { get => id; set => id=value; }

    public void Interact(string sender)
    {
        
    }
    public void Init(string id)
    {
        this.id = id;
    }
}