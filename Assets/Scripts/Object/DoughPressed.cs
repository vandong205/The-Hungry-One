using System;
using System.Collections.Generic;
using UnityEditor.XR;
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
    [SerializeField] List<ObjectData> acceptable=new();
    [SerializeField] List<IngredientID> acceptableId = new();
    private ObjectSelfCloneDispencer selfCloneDispencer;
    void OnEnable()
    {
        if(gameObject.TryGetComponent(out ObjectSelfCloneDispencer objectSelfClone))
        {
            selfCloneDispencer = objectSelfClone;
            objectSelfClone.AfterClone += HandleTakeDough;
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

}