using System;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    void Awake(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void HoverObject(bool isHovering)
    {
        _animator.SetBool("IsHover",isHovering);
    }
}
