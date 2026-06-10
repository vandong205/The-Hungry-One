using UnityEngine;

public class CursorController : MonoBehaviour
{
    void Awake(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
