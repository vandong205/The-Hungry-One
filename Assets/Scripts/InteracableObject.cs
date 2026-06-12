using UnityEngine;

public interface IInteractableObject
{
    public string ID{get;set;}
    public void Interact(string sender);

}
