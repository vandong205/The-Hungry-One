using UnityEngine;

public class TrashCan : MonoBehaviour, IInteractableObject
{
    public void Interact(ObjectData sender)
    {
        VDGlobal.Instance.PlayerController.ClearObjectInHand();
    }
}
