using System.Collections.Generic;
using UnityEngine;

public class GameDB : MonoBehaviour
{
    private static GameDB _instance;
    public static GameDB Instance{get => _instance;private set => _instance=value;}
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public GameDatabase GameDatabase;
    public ObjectData GetObjectInfo(string id)
    {
        return GameDatabase.ObjectDatabase.Get(id);
    }
    public List<ObjectData> GetAllObjectInfo()
    {
        return GameDatabase.ObjectDatabase.GetAll();
    }
}
