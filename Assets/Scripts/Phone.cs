using UnityEngine;

public class Phone : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private bool createInstance = true;
    [SerializeField] Animator animator;
    private bool isOn = false;

    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        if (createInstance)
            material = new Material(material);
    }
    private void SetPhoneState(bool on)
    {
        isOn = on;
        if (on)
        {
            Debug.Log("Đã bật đt");
            material.EnableKeyword("_EMISSION");
            material.SetColor(EmissionColorID, Color.white);
        }
        else
        {
            Debug.Log("Đã tắt đt");
            material.SetColor(EmissionColorID, Color.black);
            material.DisableKeyword("_EMISSION");
        }
    }

    public void Toggle()
    {
        SetPhoneState(!isOn);
    }
    public void TakeOut()
    {
        animator.SetBool("IsTakeOut",true);
    }
     public void PutAway()
    {
        animator.SetBool("IsTakeOut",false);
    }
}