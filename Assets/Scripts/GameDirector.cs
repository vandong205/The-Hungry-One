using System.Collections;
using UnityEngine;

public class GameDirector : MonoBehaviour
{

    private static WaitForSeconds _waitForSeconds3 = new(3f);
    [SerializeField] Phone phone;
    [SerializeField] DialogBox dialogBox;
    public void DoEvent(GameEvent gameEvent)
    {
        switch (gameEvent)
        {
            case GameEvent.PlayerEnter:
                PlayerEnterGameplayer();
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
