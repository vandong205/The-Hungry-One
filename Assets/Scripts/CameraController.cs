using Unity.Cinemachine;
using UnityEngine;
public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineBrain brain;
    public void SetBlend(CinemachineBlendDefinition.Styles blendMode,float duration)
    {
        brain.DefaultBlend = new CinemachineBlendDefinition(blendMode,duration);
    }
}
