using UnityEngine;

public interface IObjectSelfCloneReceiver
{
    public void Receive(ObjectData data,GameObject sender);
}
