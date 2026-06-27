using UnityEngine;

public class PowderMixer : MonoBehaviour,IInteractableObject
{
    private static readonly int IsTurnOnHash = Animator.StringToHash("IsTurnOn");
    [SerializeField] Animator animator;
    [SerializeField] ToggleButton toggleButton;
    [SerializeField] Renderer btnRender;
    [SerializeField] GameObject doughMixed;
    [SerializeField] ParticleSystem MakingPartical;
    [SerializeField] float makeTime = 10.0f;
    [SerializeField] ObjectData required;
    private float makeTimeLeft;
    private bool isTurnOn = false;
    private bool isMaking = false;
    private bool hasPowder=false;

    [SerializeField] string id;
    public string ID { get => id; set => id=value; }

    void Awake()
    {
        toggleButton.OnChangeValue+=HandleToggleMachine;
        btnRender.material.EnableKeyword("_EMISSION");
        btnRender.material.SetColor("_EmissionColor", Color.black);
        doughMixed.SetActive(false);
        makeTimeLeft = makeTime;
    }
    void Update()
    {

        if (isMaking)
        {
            makeTimeLeft-=Time.deltaTime;
            if(makeTimeLeft<=0) OnMakingDone();
        }
    }
    private void HandleToggleEngine()
    {
        animator.SetBool(IsTurnOnHash, isMaking);
    }
     private void HandleToggleMachine(bool isOn)
    {
        if (isOn)
        {
            btnRender.material.SetColor("_EmissionColor", Color.red);
            isMaking = true;
            if(hasPowder)
                MakingPartical.Play();
        }
        else
        {
            btnRender.material.SetColor("_EmissionColor", Color.black);
            isMaking = false;
            MakingPartical.Stop();
        }
        HandleToggleEngine();
    }
    private void StartMachine()
    {
        isMaking = true;
        toggleButton.Interact(null);
    }
    private void StopMachine()
    {
        isMaking = false;
        toggleButton.Interact(null);
    }
    private void OnMakingDone()
    {
        StopMachine();
        makeTimeLeft = makeTime;
        doughMixed.SetActive(true);
        hasPowder = false;
        
        btnRender.material.SetColor("_EmissionColor", Color.green);
    }

    public void Interact(ObjectData sender)
    {
        if(sender==null||hasPowder) return;
        if (sender.id == required.id)
        {
            hasPowder = true;
            VDGlobal.Instance.PlayerController.ClearObjectInHand();
            StartMachine();
        }
    }
}
