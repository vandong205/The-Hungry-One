using UnityEngine;

public class DoughMass : ObjectDispencer,IInteractableObject
{
    private string id;
    public string ID { get => id; set => id=value; }

    public override void Dispense(IObjectRecevier recevier)
    {
        gameObject.SetActive(false);
        recevier.Receive(spawnPoint,objectData);
    }

    public void Interact(string sender)
    {
        // throw new System.NotImplementedException();
    }
}
