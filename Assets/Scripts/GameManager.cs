using UnityEngine;

public class GameManager : MonoBehaviour
{
   [SerializeField] GameDirector gameDirector;
    void OnEnable()
    {
        GameEventHandler.OnEventReceive+=HandleGameEvent;
    }
    void Start()
    {
        VDGlobal.Instance.DisableAllAction();
        GlobalEffect.Instance.FadeIn(2f,()=>{
            VDGlobal.Instance.EnableAllAction();
            GameEventHandler.RaiseEvent(GameEvent.PlayerEnterGame);
        });
    }
    void HandleGameEvent(GameEvent gameEvent)
    {
        gameDirector.DoEvent(gameEvent);
    }
    void OnDisable()
    {
        GameEventHandler.OnEventReceive-=HandleGameEvent;
    }
}
