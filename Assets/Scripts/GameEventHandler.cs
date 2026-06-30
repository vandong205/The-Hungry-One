using System;
using UnityEngine;
public enum GameEvent
{
    PlayerEnterGame,
    PlayerTurnOnTheLight
}
public static class GameEventHandler
{
    public static Action<GameEvent> OnEventReceive;
    public static void RaiseEvent(GameEvent eventName){
        OnEventReceive?.Invoke(eventName);
    }
}
