using UnityEngine;

public class PowderMixer : MonoBehaviour
{
    private static readonly int IsTurnOnHash = Animator.StringToHash("IsTurnOn");
    [SerializeField] Animator animator;
    [SerializeField] ToggleButton toggleButton;
    [SerializeField] Renderer btnRender;
    private bool isTurnOn = false;
    private bool isMaking = false;
    void Awake()
    {
        toggleButton.OnChangeValue+=HandleToggleMachine;
        btnRender.material.EnableKeyword("_EMISSION");
        btnRender.material.SetColor("_EmissionColor", Color.black);
    }
    private void HandleToggleMachine()
    {
        animator.SetBool(IsTurnOnHash, isMaking);
    }
     private void HandleToggleMachine(bool isOn)
    {
        if (isOn)
        {
            btnRender.material.SetColor("_EmissionColor", Color.red);
            isMaking = true;
        }
        else
        {
            btnRender.material.SetColor("_EmissionColor", Color.black);
            isMaking = false;
        }
        HandleToggleMachine();
    }
    private void OnMakingDone()
    {
        
    }
}
