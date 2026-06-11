
using UnityEngine;

public abstract class ObjectDispencer:MonoBehaviour
{
    public Transform spawnPoint;
    public ObjectData objectData;
    public abstract void Dispense(IObjectRecevier recevier);
}
