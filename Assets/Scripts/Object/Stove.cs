using UnityEngine;

public class Stove : MonoBehaviour
{

    [SerializeField] float cookTime = 10.0f;
    [SerializeField] Renderer render;
    [SerializeField] ToggleButton toggleButton;
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
    }
    
}
