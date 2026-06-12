using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{

    [SerializeField] Renderer render;
    [SerializeField] ToggleButton toggleButton;
    [SerializeField] List<StoveCookSurface> cookSurfaces;
    private bool isTurnOn = false;
    void Start()
    {
        render.material.EnableKeyword("_EMISSION");
        render.material.SetColor("_EmissionColor", Color.black);
        toggleButton.OnChangeValue+=HandleToggle;
    }
    private void HandleToggle(bool isOn)
    {
        if (isOn)
        {
            render.material.SetColor("_EmissionColor", Color.white);
        }
        else
        {
            render.material.SetColor("_EmissionColor", Color.black);
        }
        isTurnOn=isOn;
        SetSurfaceState();
    }
    private void SetSurfaceState()
    {
        foreach(StoveCookSurface cookSurface in cookSurfaces)
        {
            cookSurface.isOn = isTurnOn;
        }
    }
    
}
