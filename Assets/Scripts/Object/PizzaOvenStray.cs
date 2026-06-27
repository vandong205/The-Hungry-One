using System;
using Unity.VisualScripting;
using UnityEngine;
public class PizzaOvenStray : ObjectDispencer,IInteractableObject
{
    private static readonly int IsOpenHash = Animator.StringToHash("IsOpen");
    public Action OnCookDone;
    [SerializeField] Animator animator;
    [SerializeField] ToggleButton toggleButton;
    [SerializeField] float bakeTime = 10.0f;
    [SerializeField] Renderer btnRender;
    [SerializeField] GameObject cookedPizza;
    [SerializeField] ObjectData acceptableFood;
    [SerializeField] string id;
    public string  ID { get => id; set => id=value; }
    private bool isMachineOn = false;
    private float remainTime;
    private bool isOpened = false;
    private bool CanCook => HasUncookedFood&&isMachineOn;
    private bool hasCookedFood = false;
    private bool HasUncookedFood =>cookingFood!=null;
    private GameObject cookingFood=null;

    void Awake()
    {
        remainTime=bakeTime;
        if(animator==null) animator = GetComponentInChildren<Animator>();
        toggleButton.OnChangeValue+=HandleToggleMachine;
        btnRender.material.EnableKeyword("_EMISSION");
        btnRender.material.SetColor("_EmissionColor", Color.black);
        cookedPizza.SetActive(false);

    }
    void Update()
    {
        if (CanCook)
        {
            UpdateCookTime();
        }
    }
    private void UpdateCookTime()
    {
        remainTime-=Time.deltaTime;
        if (remainTime <= 0)
        {
            OnCookFinished();
        }
    }
    private void OnCookFinished()
    {
        OnCookDone?.Invoke();
        btnRender.material.SetColor("_EmissionColor", Color.green);
        Destroy(cookingFood);
        cookingFood = null;
        remainTime=bakeTime;
        cookedPizza.SetActive(true);
        hasCookedFood = true;
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
            if(hasCookedFood) btnRender.material.SetColor("_EmissionColor", Color.green);
            else btnRender.material.SetColor("_EmissionColor", Color.red);
        }
        else
        {
            btnRender.material.SetColor("_EmissionColor", Color.black);
        }
        isMachineOn = isOn;
    }
    private void PlacePizzaIn(GameObject pizza)
    {
        cookingFood = Instantiate(pizza);
        cookingFood.transform.SetParent(transform);
        cookingFood.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }
    public override void Interact(ObjectData sender)
    {
        
    }
    public override void Dispense(IObjectRecevier recevier)
    {
        if(CanCook) return;
        if (isOpened)
        {
            if (hasCookedFood)
            {
                Debug.Log("Lấy đồ ăn");
                cookedPizza.SetActive(false);
                recevier.Receive(spawnPoint,objectData);
                hasCookedFood = false;
                btnRender.material.SetColor(
                "_EmissionColor",
                isMachineOn ? Color.red : Color.black
            );
            }
            else
            {
                if (!(hasCookedFood || HasUncookedFood))
                {
                    PickupableObject pickupable  = VDGlobal.Instance.PlayerController.GetItemInHand();
                    if (pickupable != null)
                    {
                        if (pickupable.data.id == acceptableFood.id)
                        {
                            PlacePizzaIn(pickupable.gameObject);
                            VDGlobal.Instance.PlayerController.ClearObjectInHand();
                        }
                    }
                }
               
            }
        }
        HandleToggleDoor();
    }
    private void OnDestroy()
    {
        if(toggleButton!=null)
            toggleButton.OnChangeValue -= HandleToggleMachine;
    }
}
