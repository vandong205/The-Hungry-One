using System;
using System.Collections.Generic;
using UnityEngine;
public class DoughPressed : MonoBehaviour,IInteractableObject
{

    public Action OnTakeDough;
    [SerializeField] GameObject CheeseTopping;
    [SerializeField] GameObject BasilTopping;
    [SerializeField] GameObject PineAppleTopping;
    [SerializeField] GameObject PepperTopping;
    [SerializeField] GameObject MushroomTopping;
    [SerializeField] GameObject HamTopping;
    [SerializeField] GameObject PepperoniTopping;
    [SerializeField] List<ObjectData> acceptables=new();
    [SerializeField] List<IngredientID> acceptableIds=new();
    private ObjectSelfCloneDispencer selfCloneDispencer;
    private List<IngredientID> ingredientIDs=new();
    void OnEnable()
    {
        if(gameObject.TryGetComponent(out ObjectSelfCloneDispencer objectSelfClone))
        {
            selfCloneDispencer = objectSelfClone;
            objectSelfClone.AfterClone += HandleTakeDough;
            objectSelfClone.OnInteract +=HandleAddIngredientFromPlayer;
        }
            
    }
    void OnDisable()
    {
        if (selfCloneDispencer != null)
        {
            selfCloneDispencer.AfterClone -= HandleTakeDough;
        }
    }
    public void ReceiveIngredient(IngredientID ingredientID)
    {
        switch (ingredientID)
        {
            case IngredientID.Cheese:
                CheeseTopping.SetActive(true);
                break;

            case IngredientID.Basil:
                BasilTopping.SetActive(true);
                break;

            case IngredientID.PineApple:
                PineAppleTopping.SetActive(true);
                break;

            case IngredientID.Pepper:
                PepperTopping.SetActive(true);
                break;

            case IngredientID.Mushroom:
                MushroomTopping.SetActive(true);
                break;

            case IngredientID.Ham:
                HamTopping.SetActive(true);
                break;

            case IngredientID.Pepperoni:
                PepperoniTopping.SetActive(true);
                break;
        }
        Debug.Log("Bánh đang nhận nguyên liệu: "+ingredientID);
        if(!ingredientIDs.Contains(ingredientID)) {
            Debug.Log("Đã thêm nguyên liệu "+ingredientID+"vào list");
            ingredientIDs.Add(ingredientID);
        }
    }
    public void ClearIngredients()
    {
        CheeseTopping.SetActive(false);
        BasilTopping.SetActive(false);
        PineAppleTopping.SetActive(false);
        PepperTopping.SetActive(false);
        MushroomTopping.SetActive(false);
        HamTopping.SetActive(false);
        PepperoniTopping.SetActive(false);
    }
    public void Interact(ObjectData sender)
    {
        HandleTakeDough();
    }
    private void HandleTakeDough()
    {
        Debug.Log("Thực hiện lấy bánh khỏi thớt");
        OnTakeDough?.Invoke();
    }
    private void HandleAddIngredientFromPlayer(ObjectData sender)
    {
        if(sender==null) return;
        //nếu là đặt nguyên liệu vào
        for(int i = 0; i < acceptables.Count; i++)
        {
            if (acceptables[i].id == sender.id)
            {
                ReceiveIngredient(acceptableIds[i]);
                VDGlobal.Instance.PlayerController.ClearObjectInHand();
                selfCloneDispencer.DisableNextExcute = true;
                return;
            }
        }
    }
    public List<IngredientID> GetAllIngredient()
    {
        Debug.Log("Trả về "+ingredientIDs.Count+" nguyên liệu");
        return ingredientIDs;
    }
}