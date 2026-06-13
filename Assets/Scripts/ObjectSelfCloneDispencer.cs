using System;
using UnityEngine;

public class ObjectSelfCloneDispencer:MonoBehaviour,IInteractableObject
{
    public Action AfterClone;
    public ObjectData data;
    public void Dispense(IObjectSelfCloneReceiver receiver)
    {
        
        receiver.Receive(data,gameObject);
        AfterClone?.Invoke();
        gameObject.SetActive(false);

    }

    public void Interact(ObjectData sender)
    {
        
    }
}
