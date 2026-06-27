using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
public enum NPCType
{
    
}
[Serializable]
public enum NPCChar
{
    
}
public class NPCManager : MonoBehaviour
{
    [SerializeField] List<Table> Tables;
    [SerializeField] List<GameObject> NPCprefabs;
    [SerializeField] List<NPCChar> chars;
    [SerializeField] List<SplineContainer> goToShopPaths;
    [SerializeField] List<SplineContainer> goOutShopPaths;
    private Dictionary<NPCChar,GameObject> _npcLookUp = new();
    private int chosenInPath = 0;
    private int chosenOutPath = 0;
    void Awake()
    {
        for (int i = 0; i < NPCprefabs.Count; i++)
        {
            _npcLookUp.Add(chars[i],NPCprefabs[i]);
        }
    }
    private List<NPCController> NPCs=new();
    public void SpawnNPC(NPCChar npcChar)
    {
            GameObject npc = Instantiate(_npcLookUp[npcChar]);
            if (npc.TryGetComponent<NPCController>(out var controller))
            {
                NPCs.Add(controller);
                controller.MoveAlongPath(goToShopPaths[chosenInPath]);
                chosenInPath=chosenInPath+1>=goToShopPaths.Count?0:chosenInPath+1;
            }       
    }
}
