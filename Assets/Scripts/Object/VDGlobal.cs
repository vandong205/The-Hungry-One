using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class VDGlobal : MonoBehaviour
{
    [SerializeField] InputActionReference _moveInput;
    [SerializeField] InputActionReference _interactInput;
    [SerializeField] InputActionReference mouseInput;
    [SerializeField] CinemachineBrain cinemachineBrain;
    [SerializeField] PlayerController playerController;
    private static VDGlobal _instance;
    public static VDGlobal Instance=>_instance;
    public PlayerController PlayerController=>playerController;
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public void DisableMoveAction()
    {
        _moveInput.action.Disable();
    }
    public void EnableMoveAction()
    {
        _moveInput.action.Enable();
    }
    public void DisableInteractAction()
    {
        _interactInput.action.Disable();
    }
    public void EnableInteractAction()
    {
        _interactInput.action.Enable();
    }
    public void SetCameraBlend(CinemachineBlendDefinition.Styles blendMode,float duration)
    {
        cinemachineBrain.DefaultBlend = new CinemachineBlendDefinition(blendMode,duration);
    }
    public void DisableMouse()
    {
        mouseInput.action.Disable();
    }
    public void EnableMouse()
    {
        mouseInput.action.Enable();
    }
}
