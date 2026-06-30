using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour,IInteractableObject
{
    [SerializeField] List<Light> lights;
    private bool isOn = false;
    private bool isInteractFirstTime = true;
    public void Interact(ObjectData sender)
    {
        Switch();
    }

    private void Switch()
    {
        isOn = !isOn;
        if (isInteractFirstTime)
        {
            GameEventHandler.RaiseEvent(GameEvent.PlayerTurnOnTheLight);   
            isInteractFirstTime = false;
        }
        foreach(Light light in lights)
        {
            light.enabled = isOn;
        }
    }
}
