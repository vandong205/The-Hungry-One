using System;
using UnityEngine;

public class PickupableObject : MonoBehaviour, IInteractableObject
{
    public ObjectData data;
    public void Init(ObjectData data)
    {
        this.data = data;
    }

    public virtual void Interact(ObjectData sender)
    {
        // throw new NotImplementedException();
    }
}