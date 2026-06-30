using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameDirector : MonoBehaviour
{
    [SerializeField] NPCManager nPCManager;
    private static WaitForSeconds _waitForSeconds3 = new(3f);
    [SerializeField] Phone phone;
    [SerializeField] DialogBox dialogBox;
    void Awake()
    {

    }
    public void DoEvent(GameEvent gameEvent)
    {
        switch (gameEvent)
        {
            case GameEvent.PlayerEnterGame:
                PlayerEnterGameplayer();
                return;
            case GameEvent.PlayerTurnOnTheLight:
                nPCManager.SpawnNPC(
                    NPCChar.ShopOwner,
                    VDGlobal.Instance.CameraController.transform.position - VDGlobal.Instance.CameraController.transform.forward*0.9f,
                    VDGlobal.Instance.CameraController.transform.rotation
                );
                return;
            default:
                return;
        }        
    }
    public void PlayerEnterGameplayer()
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
}
