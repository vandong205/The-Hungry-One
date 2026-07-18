using UnityEngine;
using System;
public class Dish : MonoBehaviour,IInteractableObject,IObjectRecevier
{
    public Transform pizzaSpot;
    public Action OnPizzaServe;
    private GameObject pizzaModel = null;
    public void Interact(ObjectData sender)
    {

    }
    public void OnPizzaEaten()
    {
        Destroy(pizzaModel);
    }

    public void Receive(Transform spawnPoint, ObjectData data)
    {
        if(data==null) return;
        if (pizzaModel!=null)
        {
            OnPizzaServe?.Invoke();   
            pizzaModel = Instantiate(data.prefab,pizzaSpot.position,pizzaSpot.rotation,transform);
            VDGlobal.Instance.PlayerController.ClearObjectInHand();
        }   
    }
}
