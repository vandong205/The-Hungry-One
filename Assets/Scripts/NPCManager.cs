using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;
[Serializable]
public enum NPCChar
{
    ShopOwner,
    RandomMale1,
    RandomMale2
    
}
public enum NPCRole
{
    Owner,
    TakeAway,
    Custumer
}
public class NPCManager : MonoBehaviour
{
    [SerializeField] List<Table> Tables;
    [SerializeField] List<GameObject> NPCprefabs;
    [SerializeField] List<NPCChar> chars;
    [SerializeField] List<SplineContainer> goToShopPaths;
    [SerializeField] List<SplineContainer> goOutShopPaths;
    private Dictionary<NPCChar,GameObject> _npcLookUp = new();
    private Coroutine spawnNPCCouroutine;
    private bool hasTakeAwayCustumer = false;
    private int currentCustumerNumber=0;
    private int totalSeat = 0;
    private List<NPCChar> npcPool = new();
    private List<NPCChar> usedNpc = new();
    void Awake()
    {
        for (int i = 0; i < NPCprefabs.Count; i++)
        {
            _npcLookUp.Add(chars[i],NPCprefabs[i]);
        }
        foreach(Table table in Tables)
        {
            totalSeat=table.GetTotalSeatNumber();
        }
        for (int i = 0; i < _npcLookUp.Count; i++)
        {
            npcPool.Add(_npcLookUp.ElementAt(i).Key);
        }
    }
    private Dictionary<NPCChar,NPCController> NPCs=new();
    private Dictionary<NPCController,TableSeat> UsedTable=new();
    public void SpawnNPC(NPCChar npcChar,Vector3 pos, Quaternion rot,NPCRole role)
    {
        GameObject npc = Instantiate(_npcLookUp[npcChar]);
        if (npc.TryGetComponent<NPCController>(out var controller))
        {
            if(NPCs.ContainsKey(npcChar)) NPCs.Remove(npcChar);
            Debug.Log("Đang set pos cho NPC");
            controller.OnMoveToShopDone+=OnNPCMoveToShop;
            controller.role = role;
            NPCs.Add(npcChar,controller);
            Vector3 targetPos = new(pos.x,controller.groundY,pos.z);
            Vector3 euler = rot.eulerAngles;
            euler.x=0;
            euler.z=0;
            Quaternion targetRot = Quaternion.Euler(euler);
            npc.transform.SetPositionAndRotation(targetPos, targetRot);
        }
        else
        {
            Debug.LogWarning("Thêm 1 NPC không có controller");
        }
    }
    public void SpawnCustumerNPC()
    {
        if(currentCustumerNumber>=totalSeat) return;
        int randomChar = UnityEngine.Random.Range(0, npcPool.Count);
        int randomPath = UnityEngine.Random.Range(0, goToShopPaths.Count);

        Debug.Log($"Spawn NPC: Character {randomChar + 1}, Path {randomPath + 1}");

        Vector3 startLocal = goToShopPaths[randomPath].EvaluatePosition(0f);
        Vector3 startPosWorld = goToShopPaths[randomPath].transform.TransformPoint(startLocal);

        SpawnNPC(npcPool[randomChar], startPosWorld, Quaternion.identity,NPCRole.Custumer);
        currentCustumerNumber++;
        if (NPCs.TryGetValue(npcPool[randomChar], out NPCController controller))
        {
            controller.MoveAlongPath(goToShopPaths[randomPath]);
        }
        usedNpc.Add(npcPool[randomChar]);
        npcPool.RemoveAt(randomChar);
      
    }
    public void SpawnTakeAwayNPC()
    {
        hasTakeAwayCustumer = true;
    }
    private void OnNPCMoveToShop(NPCController npc)
    {
        if (npc.role == NPCRole.Custumer)
        {
            PutCustumerOnTable(npc);
        }else if (npc.role == NPCRole.TakeAway)
        {
            
        }
    }
    private void PutCustumerOnTable(NPCController controller)
    {
        if(totalSeat==0) return;
        TableSeat availableSeat = null;
        foreach(Table table in Tables)
        {
            availableSeat = table.GetSeat();
            if(availableSeat!=null) break;
        }
        if(availableSeat==null)
        {
            Debug.Log("No more available seat");
        }else
        {

            controller.Sit(availableSeat.transform);
            availableSeat.OnSeatTaked();
            availableSeat.dish.OnPizzaServe+=controller.Eat;
            UsedTable.Add(controller,availableSeat);
            totalSeat--;
        }
       
    }

    public NPCController GetNPC(NPCChar nPCChar)
    {
        if(NPCs.TryGetValue(nPCChar,out NPCController result))
        {
            return result;
        }
        return null;
    }
    public void DestroyNPC(NPCChar nPCChar)
    {
        if(NPCs.TryGetValue(nPCChar,out NPCController result))
        {
            Destroy(result.gameObject);
            NPCs.Remove(nPCChar);
        }
    }
    public void RemoveNPCInnDict(NPCChar nPCChar)
    {
        if(_npcLookUp.ContainsKey(nPCChar))
        {
            _npcLookUp.Remove(nPCChar);
        }
        if (npcPool.Contains(nPCChar))
        {
            npcPool.Remove(nPCChar);
        }
    }
    public void SpawnNPCRoutine()
    {
        spawnNPCCouroutine = StartCoroutine(SpawnNPCCouroutine());
    }
    IEnumerator SpawnNPCCouroutine()
    {
        yield return null;
    }
    public void StopSpawnNPCRoutine()
    {
        StopCoroutine(spawnNPCCouroutine);
    }
}
