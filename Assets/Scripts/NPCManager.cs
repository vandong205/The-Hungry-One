using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;
[Serializable]
public enum NPCChar
{
    ShopOwner
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
    public void SpawnNPC(NPCChar npcChar,Vector3 pos, Quaternion rot)
    {
            GameObject npc = Instantiate(_npcLookUp[npcChar]);
            if (npc.TryGetComponent<NPCController>(out var controller))
            {
                Debug.Log("Đang set pos cho NPC");
                NPCs.Add(controller);
                Vector3 targetPos = new(pos.x,controller.groundY,pos.z);
                Vector3 euler = rot.eulerAngles;
                euler.x=0;
                euler.z=0;
                Quaternion targetRot = Quaternion.Euler(euler);
                npc.transform.SetPositionAndRotation(targetPos, targetRot);
        }       
    }
}
