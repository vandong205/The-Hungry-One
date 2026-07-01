using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;
[RequireComponent(typeof(Animator))]
public class NPCController : MonoBehaviour,IInteractableObject
{
    public Action<NPCController> OnMoveToShopDone;

    private static readonly int MoveHash = Animator.StringToHash("Move");
    [SerializeField] SplineAnimate splineAnimate;
    [SerializeField] CinemachineCamera cinemachine;
    [SerializeField] GameEvent gameEvent;
    [SerializeField] float sitYpos = 0.17f;
    public float groundY;
    private bool isZoomed = false;
    private Animator animator;
    private bool isGoAway = false;

    private bool isMoving = false;
    public NPCRole role;
    void Awake()
    {
        animator = GetComponent<Animator>();
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
        animator.SetBool(MoveHash, isMoving);
        splineAnimate.Pause();
    }
   private void OnTriggerEnter(Collider other)
    {
        Debug.Log("NPC "+role+" đi vào "+other.tag);
        if(isGoAway) return;
        if (other.CompareTag("CustumerBreakPoint"))
        {
            if (role == NPCRole.Custumer)
            {
                StopAtBreakPoint();
            }
        }
        else if (other.CompareTag("TakeAwayBreakPoint"))
        {
            if (role == NPCRole.TakeAway)
            {
                StopAtBreakPoint();
            }
        }
    }
    private void StopAtBreakPoint()
    {
        StopMove();
        OnMoveToShopDone?.Invoke(this);
    }
    public void Sit(Transform seatTranform)
    {
        if(role!=NPCRole.Custumer) return;
        isMoving = false;
        animator.SetTrigger("Sit");
        transform.position = new Vector3(seatTranform.position.x,sitYpos,seatTranform.position.z);
        transform.rotation = seatTranform.rotation;
    }

    public void Interact(ObjectData sender)
    {
        GameEventHandler.RaiseEvent(gameEvent);
        if (cinemachine != null)
        {
            cinemachine.Priority=isZoomed?0:100;
            isZoomed = !isZoomed;
        }
    }
}
