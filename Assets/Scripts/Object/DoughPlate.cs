using UnityEngine;

public class DoughPlate : MonoBehaviour,IInteractableObject
{
    [SerializeField] GameObject doughPressed;
    [SerializeField] ObjectData required;
    [SerializeField] DoughPressed dough;
    private string id;
    public string ID { get => id; set => id=value; }
    public bool hasDough = false;
    public void Interact(string sender)
    {
        if(hasDough) return;
        if (sender == required.id)
        {
            doughPressed.SetActive(true);
            VDGlobal.Instance.PlayerController.ClearObjectInHand();
            hasDough = true;
        }
    }

    void Awake()
    {
        doughPressed.SetActive(false);
    }
    public void ReceiveIngredient(IngredientID ingredient)
    {
        if(!hasDough) return;
        dough.ReceiveIngredient(ingredient);
    }
}
