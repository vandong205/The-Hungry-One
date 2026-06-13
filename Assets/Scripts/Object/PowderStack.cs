using UnityEngine;

public class PowderStack : ObjectDispencer,IInteractableObject
{
    public string ID { get => objectData.id; set => objectData.id=value; }
    public override void Dispense(IObjectRecevier recevier)
    {
        recevier.Receive(spawnPoint,objectData);
    }

    public override void Interact(ObjectData sender)
    {
       
    }
}
