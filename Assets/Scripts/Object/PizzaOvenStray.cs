using System;
using Unity.VisualScripting;
using UnityEngine;
public class PizzaOvenStray : MonoBehaviour,IInteractableObject
{
    private static readonly int IsOpenHash = Animator.StringToHash("IsOpen");
    public Action OnCookDone;
    public Action OnTakeCakeOut;
    [SerializeField] Animator animator;
    [SerializeField] ToggleButton toggleButton;
    [SerializeField] float bakeTime = 10.0f;
    [SerializeField] Renderer btnRender;
    
    [SerializeField] string id;
    public string  ID { get => id; set => id=value; }
    private float remainTime;
    private bool isFree = true;
    private bool isOpened = false;
    private bool isCooking = false;

    void Awake()
    {
        remainTime=bakeTime;
        if(animator==null) animator = GetComponentInChildren<Animator>();
        toggleButton.OnChangeValue+=HandleToggleMachine;
        btnRender.material.EnableKeyword("_EMISSION");
        btnRender.material.SetColor("_EmissionColor", Color.black);

    }
    void Update()
    {
        if (isCooking)
        {
            UpdateCookTime();
        }
    }
    private void UpdateCookTime()
    {
        remainTime-=Time.deltaTime;
        if (remainTime <= 0)
        {
            isCooking = false;
            OnCookFinished();
        }
    }
    private void OnCookFinished()
    {
        OnCookDone?.Invoke();
        btnRender.material.SetColor("_EmissionColor", Color.green);
    }
    private void OnTakePizzaOut()
    {
        OnTakeCakeOut?.Invoke();
        isFree = true;
        remainTime = bakeTime;
    }
    private void HandleToggleDoor()
    {
        isOpened = !isOpened;
        animator.SetBool(IsOpenHash, isOpened);
    }
    private void HandleToggleMachine(bool isOn)
    {
        Debug.Log("Toggle Oven");
        if (isOn)
        {
            btnRender.material.SetColor("_EmissionColor", Color.red);
            isCooking = true;
        }
        else
        {
            btnRender.material.SetColor("_EmissionColor", Color.black);
            isCooking = false;
        }
    }
    public void Interact(string sender)
    {
        if (isFree)
        {
            HandleToggleDoor();
        }
        else
        {
            
        }
    }
}
