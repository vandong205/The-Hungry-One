using System.Collections.Generic;
using UnityEngine;
public class DoughPlate : MonoBehaviour,IInteractableObject
{
    [SerializeField] ObjectData required;
    [SerializeField] DoughPressed dough;
    [SerializeField] ObjectData doughPressedData;
    private string id;
    public string ID { get => id; set => id=value; }
    public bool hasDough = false;
    public void Interact(ObjectData sender)
    {
        if(sender==null) return;
        if (sender.id == required.id||sender.id == doughPressedData.id)
        {
            dough.gameObject.SetActive(true);
            VDGlobal.Instance.PlayerController.ClearObjectInHand();
            hasDough = true;
        }
    }

    void Awake()
    {
        dough.gameObject.SetActive(false);
        dough.OnTakeDough+=OnTakeDough;
    }
    public void ReceiveIngredient(IngredientID ingredient)
    {
        if(!hasDough) return;
        dough.ReceiveIngredient(ingredient);
    }
    private void OnTakeDough()
    {
        if(!hasDough) return;
        Debug.Log("Đã lấy bánh khỏi thớt");
        hasDough = false;
        dough.gameObject.SetActive(false);
    }
}
