using UnityEngine;

public class GameManager : MonoBehaviour
{
   [SerializeField] GlobalEffect effect;
   [SerializeField] GameDirector gameDirector;

    void OnEnable()
    {
        GameEventHandler.OnEventReceive+=HandleGameEvent;
    }
    void Start()
    {
        VDGlobal.Instance.DisableAllAction();
        effect.FadeIn(callback:()=>{
            VDGlobal.Instance.EnableAllAction();
            GameEventHandler.RaiseEvent(GameEvent.PlayerEnter);
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
