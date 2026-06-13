using UnityEngine;

public class DoughMass : PickupableObject,IInteractableObject
{
    public string ID { get => data.id; set => data.id=value; }
}
