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
    [SerializeField] private GameDatabase gameDatabase;
    public ObjectData GetObjectInfo(string id)
    {
        return gameDatabase.ObjectDatabase.Get(id);
    }
    public List<ObjectData> GetAllObjectInfo()
    {
        return gameDatabase.ObjectDatabase.GetAll();
    }
}
