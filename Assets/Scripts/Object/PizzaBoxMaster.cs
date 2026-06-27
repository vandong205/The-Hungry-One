using UnityEngine;

public class PizzaBoxMaster : ObjectDispencer
{
    private static readonly int IsOpenHash = Animator.StringToHash("IsOpen");
    [SerializeField] GameObject model;
   [SerializeField] GameObject pizzaModel;
   [SerializeField] Animator animator;
   [SerializeField]  ObjectData pizzaData;
   [SerializeField] ObjectData pizzaBoxData;
    private bool isOpen = false;
    private bool hasPizza = false;
    private bool isActive = false;
    private int DisableNextExcute = -1;
    void Awake()
    {
        model.SetActive(false);
        pizzaModel.SetActive(false);
    }
    public override void Dispense(IObjectRecevier recevier)
    {
        if(!hasPizza) return;
        if (DisableNextExcute>0)
        {
            DisableNextExcute -=1;
            return;
        }
        model.SetActive(false);
        isActive = false;
        hasPizza = false;
        pizzaModel.SetActive(false);
        recevier.Receive(spawnPoint,objectData);

    }

    public override void Interact(ObjectData sender)
    {
        if (sender != null)
        {
            if (sender.id == pizzaBoxData.id)
            {
                model.SetActive(true);
                isActive = true;
                VDGlobal.Instance.PlayerController.ClearObjectInHand();
            }
            if (isOpen&&isActive)
            {
                if (sender.id == pizzaData.id)
                {
                    VDGlobal.Instance.PlayerController.ClearObjectInHand();
                    pizzaModel.SetActive(true);
                    hasPizza = true;
                    DisableNextExcute = 2;
                    return;
                }
            }
        }
          if(isActive)
            ToggleOpenCap();
    }
    private void ToggleOpenCap()
    {
        isOpen=!isOpen;
        animator.SetBool(IsOpenHash, isOpen);
    }
}
