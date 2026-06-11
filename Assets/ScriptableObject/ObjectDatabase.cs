using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "ObjectDatabase", menuName = "Scriptable Objects/ObjectDatabase")]
public class ObjectDatabase : ScriptableObject
{
    [SerializeField] private List<ObjectData> objects=new();
    private Dictionary<string,ObjectData> _lookupDict=new();
    private void OnValidate()
    {
        foreach(ObjectData data in objects)
        {
            _lookupDict.Add(data.id,data);
        }
    }
    public ObjectData Get(string id)
    {
        if(_lookupDict.Count==0) BuildLookup();
        if(_lookupDict.TryGetValue(id,out ObjectData data))
        {
            return data;
        }
        return null;
    }
    private void BuildLookup()
{
    _lookupDict = new();

    foreach(ObjectData data in objects)
    {
        _lookupDict[data.id] = data;
    }
}
    public List<ObjectData> GetAll()
    {
        return objects;
    }
}
