using UnityEngine;

public class PizzaBoxStack : ObjectDispencer, IInteractableObject
{
    [SerializeField] string id;
    public string ID { get => id; set => id=value; }

    public override void Interact(ObjectData sender)
    {
        // Dispense();
    }

    public override void Dispense(IObjectRecevier recevier)
    {
       recevier.Receive(spawnPoint,objectData);
    }
}
