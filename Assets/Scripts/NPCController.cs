using System;
using UnityEngine;
using UnityEngine.Splines;
[RequireComponent(typeof(Animator))]
public class NPCController : MonoBehaviour
{
    public Action OnMoveToShopDone;
    public Action OnMoveToOutSideDone;

    private static readonly int MoveHash = Animator.StringToHash("Move");
    [SerializeField] SplineAnimate splineAnimate;
    public float groundY;
    private Animator animator;
    private bool isMoving = false;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter(Collider other)
    {
        StopMove();
        if (other.CompareTag("ShopEntry"))
        {
            OnMoveToShopDone?.Invoke();
        }else if (other.CompareTag("NPCEndPoint"))
        {
            OnMoveToOutSideDone.Invoke();
        }
    }
    public void MoveAlongPath(SplineContainer path)
    {
        splineAnimate.Container = path;
        isMoving = true;
        animator.SetBool(MoveHash, isMoving);
        splineAnimate.Play();
    }
    private void StopMove()
    {
        isMoving = false;
        animator.SetBool("Move",isMoving);
    }
    public void Sit(Vector3 pos)
    {
        isMoving = false;
        animator.SetTrigger("Sit");
        transform.position = pos;

    }
}
