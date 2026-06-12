using UnityEngine;

public class PowderStack : ObjectDispencer,IInteractableObject
{
    private string id;
    public string ID { get => id; set => id=value; }
    private bool hasPowder = false;
    public override void Dispense(IObjectRecevier recevier)
    {
        recevier.Receive(spawnPoint,objectData);
    }

    public void Interact(string sender)
    {
       
    }
}
