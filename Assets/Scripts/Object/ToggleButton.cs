using System;
using UnityEngine;

public class ToggleButton : MonoBehaviour,IInteractableObject
{
    public Action<bool> OnChangeValue;
    public bool isTurnOn = false;
    private string id;
    public string ID { get => id; set => id=value; }

    public void Interact(string sender)
    {
        isTurnOn=!isTurnOn;
        OnChangeValue?.Invoke(isTurnOn);
    }
}
