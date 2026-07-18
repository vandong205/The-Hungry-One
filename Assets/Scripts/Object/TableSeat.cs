using UnityEngine;

public class TableSeat : MonoBehaviour
{
    public Dish dish;
    public OrderPaper orderPaper;
    public bool isFree = true;
    void Awake()
    {
        orderPaper.gameObject.SetActive(false);
        dish.gameObject.SetActive(false);
    }
    public void OnSeatTaked()
    {
        isFree = false;
        orderPaper.gameObject.SetActive(true);
        dish.gameObject.SetActive(true);
    }
}
