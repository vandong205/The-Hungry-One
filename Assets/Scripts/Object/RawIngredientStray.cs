using UnityEngine;

public class RawIngredientStray : ObjectDispencer,IInteractableObject
{
    private string id;
    public string ID { get => id; set => id=value; }

    public override void Dispense(IObjectRecevier recevier)
    {
        recevier.Receive(spawnPoint,objectData);
    }

    public override void Interact(ObjectData sender)
    {
        // throw new System.NotImplementedException();
    }
}
