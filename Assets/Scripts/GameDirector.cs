using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public class GameDirector : MonoBehaviour
{
    [SerializeField] NPCManager nPCManager;
    private static WaitForSeconds _waitForSeconds3 = new(3f);
    [SerializeField] Phone phone;
    [SerializeField] DialogBox dialogBox;

    public void DoEvent(GameEvent gameEvent)
    {
        switch (gameEvent)
        {
            case GameEvent.PlayerEnterGame:
                PlayerEnterGameplayer();
                return;
            case GameEvent.PlayerTurnOnTheLight:
                VDGlobal.Instance.CameraController.SetBlend(Unity.Cinemachine.CinemachineBlendDefinition.Styles.EaseIn,0.5f);
                nPCManager.SpawnNPC(
                    NPCChar.ShopOwner,
                    VDGlobal.Instance.CameraController.transform.position - VDGlobal.Instance.CameraController.transform.forward*0.9f,
                    VDGlobal.Instance.CameraController.transform.rotation,
                    NPCRole.Owner
                );
                return;
            case GameEvent.PlayerEnterShop:
                PlayerEnterShop();
                return;
            case GameEvent.PlayerTalkToShopOwner:
                VDGlobal.Instance.DisableMoveAction();
                VDGlobal.Instance.DisableInteractAction();
                dialogBox.StartDialog(new[]
                {
                    new DialogLine("Quản lý","Cậu là ai sao lạ ở đây?"),
                    new DialogLine("","Tôi là người mới"),
                }, () =>{
                    GlobalEffect.Instance.FadeOut(callback:() =>
                    {
                        VDGlobal.Instance.EnableAllAction();
                        nPCManager.DestroyNPC(NPCChar.ShopOwner);
                        nPCManager.RemoveNPCInnDict(NPCChar.ShopOwner);
                        GlobalEffect.Instance.FadeIn();
                        nPCManager.SpawnCustumerNPC();
                    });
                });
                return;
            default:
                return;
        }        
    }
    void PlayerEnterGameplayer()
    {
        StartCoroutine(PlayerEnterGameplayerCoroutine());
    }

    IEnumerator PlayerEnterGameplayerCoroutine()
    {
        yield return _waitForSeconds3;
        phone.TakeOut();
        dialogBox.StartDialog(new[]
        {
            new DialogLine("Ông chủ", "Alo, cậu đến nơi nhận việc chưa?"),
            new DialogLine("", "Dạ, em vừa tới rồi."),
            new DialogLine("Ông chủ", "Được. Quản lý đang đợi trong đó, vào làm ngay đi. Nhớ làm đúng quy định."),
            new DialogLine("", "Vâng, em hiểu rồi.")
        },
        phone.PutAway);
    }
    void PlayerEnterShop()
    {
        StartCoroutine(PlayerEnterShopCoroutine());
    }
    
    IEnumerator PlayerEnterShopCoroutine()
    {
        yield return new WaitForSeconds(1);
        dialogBox.StartDialog(new[]
        {
            new DialogLine("", "Quán tối quá, lẽ nào quản lý chưa tới"),
            new DialogLine("", "Mình cần bật đèn lên trước đã ..."),
        });
    }
}
