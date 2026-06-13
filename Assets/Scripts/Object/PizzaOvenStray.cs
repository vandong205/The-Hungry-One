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
    private float remainTime;
    private bool isFree = true;
    private bool isOpened = false;
    private bool isCooking = false;
    private bool hasCookedFood = false;
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
        hasCookedFood = true;
        btnRender.material.SetColor("_EmissionColor", Color.green);
        Destroy(cookingFood);
        cookedPizza.SetActive(true);
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
        if(isCooking) return;
        if (isOpened)
        {
            if (hasCookedFood)
            {
                Debug.Log("Lấy đồ ăn");
                cookedPizza.SetActive(false);
                recevier.Receive(spawnPoint,objectData);
                hasCookedFood = false;
            }
            else
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
        HandleToggleDoor();
    }
}
