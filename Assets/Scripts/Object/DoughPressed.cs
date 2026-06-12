using UnityEngine;

public class DoughPressed : MonoBehaviour
{
    [SerializeField] GameObject CheeseTopping;
    [SerializeField] GameObject BasilTopping;
    [SerializeField] GameObject PineAppleTopping;
    [SerializeField] GameObject PepperTopping;
    [SerializeField] GameObject MushroomTopping;
    [SerializeField] GameObject HamTopping;
    [SerializeField] GameObject PepperoniTopping;

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
}