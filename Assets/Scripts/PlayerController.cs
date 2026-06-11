using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using DG.Tweening;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Properties
    [Header("Reference")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] InputActionReference _moveAction;
    [SerializeField] CursorController _cursorController;
    [SerializeField] Camera _playerCamera;
    [Header("Interact")]
    [SerializeField] LayerMask _objectLayer;
    [SerializeField] private float _detectDistance=10;
    [SerializeField] private InputActionReference _interactAction;
    [SerializeField] private InputActionReference _dropAction;

    [SerializeField] Transform _handSlot;


    [Header("Movement")]
    private CharacterController characterController;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravity = -12f;
    [SerializeField] private float initialFallVelocity = -0.5f;
    private Vector2 moveInput;
    private float verticalVelocity;
    private bool isGrounded;
    private PickupableObject holdingItem;

    #endregion

    #region  UnityEngine

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        _moveAction.action.performed += StoreMovementInput;
        _moveAction.action.canceled += StoreMovementInput;
        _dropAction.action.performed += HandleDrop;
    }
    private void OnDisable()
    {
        _moveAction.action.performed -= StoreMovementInput;
        _moveAction.action.canceled -= StoreMovementInput;
        _dropAction.action.performed -= HandleDrop;

    }
    void Update()
    {
        HandleGravity();
        HandleMovement();
        CheckObject();
        
    }
    #endregion
    
    #region  Private Method
    private void HandleMovement()
    {
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 move =
            cameraForward * moveInput.y +
            cameraRight * moveInput.x;

        move = Vector3.ClampMagnitude(move, 1f);

        float currentSpeed = _moveSpeed;

        Vector3 velocity =
            move * currentSpeed;

        velocity.y = verticalVelocity;

        characterController.Move(
            velocity * Time.deltaTime
        );
    }
    private void HandleGravity()
    {
        isGrounded = characterController.isGrounded;

        if (isGrounded)
        {
            verticalVelocity = initialFallVelocity;
        }
        else
        {
            verticalVelocity += _gravity * Time.deltaTime;
        }
    }
    private void StoreMovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput.magnitude < 0.1f)
        {
            moveInput = Vector2.zero;
        }
    }
       private void CheckObject()
    {
        bool lookingAtObject = false;

        Ray ray =
            _playerCamera.ViewportPointToRay(
                new Vector3(0.5f, 0.5f, 0f)
            );

        if (
            Physics.Raycast(
                ray,
                out RaycastHit hit,
                _detectDistance,
                _objectLayer
            )
        )
        {
            lookingAtObject = true;

            if (
                _interactAction.action
                    .WasPerformedThisFrame()
            )
            {
                Debug.Log("Hành động tương tác được thực hiện");
                if (hit.collider.TryGetComponent<IInteractableObject>(out var interactable))
                {
                    HandleInteract(interactable);
                }
            }
        }

        _cursorController.HoverObject(
            lookingAtObject
        );
    }
   private void HandleInteract(IInteractableObject interactable)
    {
        if (interactable == null)
            return;
        if (interactable is PickupableObject pickupable)
        {
            Debug.Log(pickupable.ID);
            MonoBehaviour behaviour =
                interactable as MonoBehaviour;
            if (behaviour != null)
            {
                Debug.Log(GameDB.Instance);

                ObjectData data =
                    GameDB.Instance.GetObjectInfo(
                        pickupable.ID
                    );
                if (data != null)
                {
                    if(holdingItem!=null) DropItem();
                    HoldItem(
                        data.InHandPos,
                        data.InHandRot,
                        behaviour.transform,
                        pickupable
                    );
                }
                else
                {
                    List<ObjectData> datas  = GameDB.Instance.GetAllObjectInfo();
                    foreach(ObjectData data1 in datas)
                        {
                            Debug.Log(data1.id);
                        }
                }
            }
        }else if(interactable is ObjectDispencer dispencer)
        {
            ReceiveObject(dispencer);            
        }else interactable.Interact("Player");
    }
    private void HandleDrop(InputAction.CallbackContext context)
    {
        DropItem();
    }
    public void HoldItem(Vector3 pos,Vector3 rot,Transform item,PickupableObject data)
    {
        if(data==null) return;
        holdingItem = data;
        if (item.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
        item.SetParent(_handSlot);
        item.SetLocalPositionAndRotation(pos,Quaternion.Euler(rot));
    }
    public void DropItem()
    {
        if (holdingItem == null)
            return;
        Transform item =
            holdingItem.transform;

        item.SetParent(null);

        if (item.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;

        }
           
         rb.linearVelocity = Vector3.zero;

        rb.angularVelocity = Vector3.zero;
        Vector3 throwDirection =
         _playerCamera.transform.forward;

        throwDirection.y = 0f;

        throwDirection.Normalize();

        throwDirection += Vector3.up * 0.2f;

        rb.AddForce(
            throwDirection.normalized * 2f,
            ForceMode.Impulse
        );

        holdingItem = null;
    }
    public void ReceiveObject(ObjectDispencer dispencer)
    {
        ObjectData data  = dispencer.objectData;
        if(holdingItem!=null)
        {
            if(holdingItem.ID==data.id) return;
            else DropItem();
        }
        GameObject obj = Instantiate(
            dispencer.objectData.prefab,
            dispencer.spawnPoint.position,
            Quaternion.identity);
        Vector3 endPos = _handSlot.TransformPoint(data.InHandPos);

        Quaternion endRot =_handSlot.rotation *
        Quaternion.Euler(data.InHandRot);
        Sequence seq = DOTween.Sequence();
        seq.Join(
            obj.transform.DOMove(endPos, 0.4f)
        );

        seq.Join(
            obj.transform.DORotateQuaternion(endRot, 0.4f)
        );

        seq.OnComplete(() =>
        {
            PickupableObject pickable = obj.AddComponent<PickupableObject>();
            pickable.Init(data.id);

            holdingItem = pickable;

            obj.transform.SetParent(_handSlot, true);

            // Đảm bảo local transform đúng tuyệt đối
            obj.transform.localPosition = data.InHandPos;
            obj.transform.localEulerAngles = data.InHandRot;
        });
    }
    #endregion
}
