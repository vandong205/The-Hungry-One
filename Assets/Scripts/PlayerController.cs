using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using DG.Tweening;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour,IObjectRecevier,IObjectSelfCloneReceiver
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
    
    public PickupableObject HoldingItem {get=>holdingItem;private set {
        if(holdingItem==value) return; 
        holdingItem=value;
        if(holdingItem==null) Debug.Log("Player đã bỏ đồ vật đang cầm");
        else Debug.Log("Player đang cầm "+holdingItem.data.name+" với ID: "+holdingItem.data.id);

        }
    }

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
                if (hit.collider.TryGetComponent<PickupableObject>(out var pickup))
                {
                    Debug.Log("Tương tác với: PickupableObject");
                    HandleInteract(pickup);
                }
                else if (hit.collider.TryGetComponent<ObjectDispencer>(out var dispencer))
                {
                    Debug.Log("Tương tác với: ObjectDispencer");
                    HandleInteract(dispencer);
                }
                else if(hit.collider.TryGetComponent<ObjectSelfCloneDispencer>(out var clonedispencer)){
                    Debug.Log("Tương tác với: ObjectSelfCloneDispencer");
                    HandleInteract(clonedispencer);
                }
                else if (hit.collider.TryGetComponent<IInteractableObject>(out var interactable))
                {
                    Debug.Log("Tương tác với: IInteractableObject");
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
        Debug.Log("Truyền tương tác từ "+holdingItem+" tới "+interactable);
        interactable.Interact(holdingItem==null?null:holdingItem.data);
        if (interactable is PickupableObject pickupable)
        {
            DropItem();
            ObjectData data = pickupable.data;
            if (data != null)
            {
                HoldItem(
                    data,
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
        }else if(interactable is ObjectDispencer dispencer)
        {
            Debug.Log("Yêu cầu phát object từ ObjectDispencer "+dispencer.name);
            dispencer.Dispense(this);            
        }else if (interactable is ObjectSelfCloneDispencer cloneDispencers)
        {
            Debug.Log("Yêu cầu phát object từ ObjectSelfCloneDispencer "+cloneDispencers.name);
            cloneDispencers.Dispense(this);
        }
    }
    private void HandleDrop(InputAction.CallbackContext context)
    {
        DropItem();
    }
    public void HoldItem(ObjectData data,PickupableObject item)
    {
        if(data==null) return;
        HoldingItem = item;
        if (item.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }
        item.transform.SetParent(_handSlot);
        item.transform.SetLocalPositionAndRotation(data.InHandPos,Quaternion.Euler(data.InHandRot));
    }
    public void DropItem()
    {
        if (holdingItem == null)
            return;
        Transform item =
            holdingItem.transform;

        item.SetParent(null);
       
        Vector3 throwDirection =
         _playerCamera.transform.forward;

        throwDirection.y = 0f;

        throwDirection.Normalize();

        throwDirection += Vector3.up * 0.2f;
        
        if (item.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
            rb.detectCollisions = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
             rb.AddForce(
            throwDirection.normalized * 2f,
            ForceMode.Impulse
        );
        }
        Collider[] cols = holdingItem.GetComponentsInChildren<Collider>();
        foreach(var col in cols)
        {
            col.enabled = true;
        }

        HoldingItem = null;
    }
    public void ClearObjectInHand()
    {
        if(holdingItem==null) return;
        Destroy(holdingItem.gameObject);
        HoldingItem = null;
    }

    public void Receive(Transform spawnPoint, ObjectData data)
    {
        if(data==null) return;
        Debug.Log("Player nhận object: "+data.Name);
        if(holdingItem!=null)
        {
            if(holdingItem.data.id==data.id) return;
            DropItem();
        }
        GameObject obj = Instantiate(data.prefab);
            obj.transform.position = spawnPoint.position;
        obj.transform.localScale*=data.ScaleMultiplier;
        if (obj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = true;
        }
        Collider[] cols = obj.GetComponentsInChildren<Collider>();
        foreach(var col in cols)
        {
            col.enabled = false;
        }
        obj.transform.SetParent(_handSlot, true);
        obj.transform.DOLocalMove(data.InHandPos, 0.4f);
        obj.transform.DOLocalRotate(data.InHandRot, 0.4f)
            .OnComplete(() =>
            {
                PickupableObject pickable = obj.AddComponent<PickupableObject>();
                pickable.Init(data);
                HoldItem(data,pickable);
            });
    }
    public PickupableObject GetItemInHand()
    {
        return holdingItem;
    }

    public void Receive(ObjectData data,GameObject sender)
    {
        GameObject clone = Instantiate(sender);
        Destroy(clone.GetComponent<ObjectSelfCloneDispencer>());
        clone.transform.localScale*=data.ScaleMultiplier;
        if (!clone.TryGetComponent<PickupableObject>(out var pickupable))
        {
            pickupable = clone.AddComponent<PickupableObject>();
            pickupable.Init(data);
        }
        HoldingItem = pickupable;
        HoldItem(data,pickupable);
    }
    #endregion
}
