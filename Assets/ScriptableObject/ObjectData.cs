using UnityEngine;

[CreateAssetMenu(fileName = "ObjectData", menuName = "Scriptable Objects/ObjectData")]
public class ObjectData : ScriptableObject
{
    public GameObject prefab;
    public string id;
    public Vector3 InHandPos;
    public Vector3 InHandRot;
    public string Name;
}
