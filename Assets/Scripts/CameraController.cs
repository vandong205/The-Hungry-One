using Unity.Cinemachine;
using UnityEngine;
[RequireComponent(typeof(CinemachineCamera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineBrain brain;
    private CinemachineCamera _self;
    void Awake()
    {
        _self = GetComponent<CinemachineCamera>();
    }
    public void SetBlend(CinemachineBlendDefinition.Styles blendMode,float duration)
    {
        brain.DefaultBlend = new CinemachineBlendDefinition(blendMode,duration);
    }


}
