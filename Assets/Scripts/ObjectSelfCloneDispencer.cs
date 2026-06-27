using System;
using UnityEngine;

public class ObjectSelfCloneDispencer:MonoBehaviour,IInteractableObject
{
    public Action AfterClone;
    public Action<ObjectData> OnInteract;
    public ObjectData data;
    public bool DisableNextExcute = false;
    public void Dispense(IObjectSelfCloneReceiver receiver)
    {
        if(DisableNextExcute) {
            DisableNextExcute  =false;
            return;
        }
        receiver.Receive(data,gameObject);
        AfterClone?.Invoke();
        gameObject.SetActive(false);
    }

    public void Interact(ObjectData sender)
    {
        OnInteract?.Invoke(sender);
    }
}
