using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] List<TableSeat> seats;
    public TableSeat GetSeat()
    {
        foreach(TableSeat seat in seats)
        {
            if(seat.isFree) return seat;
        }
        return null;
    }

}
