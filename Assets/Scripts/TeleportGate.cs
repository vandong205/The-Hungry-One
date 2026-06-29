using System.Collections;
using System;
using UnityEngine;
using Unity.Cinemachine;

public class TeleportGate : MonoBehaviour
{
    [SerializeField] Transform targetPos;
    private bool isPlayer = false;
    private GameObject _currentEntity;    
    public void Teleport()
    {
        if(_currentEntity==null) return;
        if (isPlayer)
        {
            GlobalEffect.Instance.FadeOut(callback: () =>
            {
                StartCoroutine(TeleportSequence(() =>
                {
                    GlobalEffect.Instance.FadeIn();
                
                }));
            });
        }
        else
        {
            StartCoroutine(TeleportSequence());
        }

    }
    private IEnumerator TeleportSequence(Action OnDone=null)
    {
        PlayerController controller = _currentEntity.GetComponent<PlayerController>();
        if (isPlayer)
        {
            VDGlobal.Instance.SetCameraBlend(CinemachineBlendDefinition.Styles.Cut,0);
            controller.enabled = false;
        }
        Rigidbody rb = _currentEntity.GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        _currentEntity.transform.position = targetPos.position;
         if (rb != null)
        {
            rb.isKinematic = false;
        }
        if (isPlayer)
        {
            CharacterController charControl = _currentEntity.GetComponent<CharacterController>();
            if(charControl!=null) 
                while(!charControl.isGrounded) yield return null;
            controller.enabled = true;
        }
        OnDone?.Invoke();
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Phat hien vat the di vao cong");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Là player");
            isPlayer = true;
        }
        _currentEntity = other.gameObject;
        Teleport();
    }
}
