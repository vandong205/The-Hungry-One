
using UnityEngine;

public abstract class ObjectDispencer:MonoBehaviour,IInteractableObject
{
    public Transform spawnPoint;
    public ObjectData objectData;
    public abstract void Dispense(IObjectRecevier recevier);

    public abstract void Interact(ObjectData sender);
}
