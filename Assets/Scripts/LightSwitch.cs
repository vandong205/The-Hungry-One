using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    [SerializeField] List<Light> lights;
    private bool isOn = false;
    public void Switch()
    {
        isOn = !isOn;
        foreach(Light light in lights)
        {
            light.enabled = isOn;
        }
    }
}
