using UnityEngine;

public class PizzaShopDoor : MonoBehaviour
{
    private static readonly int IsOpenHash = Animator.StringToHash("IsOpen");
    [SerializeField] Animator _animator;
    void OnTriggerEnter(Collider other)
    {
        _animator.SetBool(IsOpenHash, true);
    }
    void OnTriggerExit(Collider other)
    {
        _animator.SetBool(IsOpenHash,false);
    }
}
