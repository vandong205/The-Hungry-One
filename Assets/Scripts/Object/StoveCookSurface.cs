using System.Collections.Generic;
using UnityEngine;

public class StoveCookSurface : MonoBehaviour, IInteractableObject
{
    [SerializeField] List<ObjectData> requireds;
    [SerializeField] List<ObjectData> results;

    [SerializeField] float cookTime = 10f;

    private bool hasUnCookFood=false;
    public bool isOn=false;
    private bool hasCookedFood=false;

    private float remainTime;

    private int isMakingIndex = -1;

    private GameObject currentFood;

    private string id;
    public string ID { get => id; set => id = value; }

    void Awake()
    {
        remainTime = cookTime;

        if (requireds.Count != results.Count || requireds.Count == 0)
        {
            Debug.LogWarning("Số nguyên liệu và kết quả không khớp.");
        }
    }

    void Update()
    {
        if (!isOn||!hasUnCookFood)
            return;
        Debug.Log("Đang nấu bếp "+gameObject.name);
        remainTime -= Time.deltaTime;
        if (remainTime <= 0)
        {
            OnCookDone();
        }
    }

    public void Interact(ObjectData sender)
    {
        // Bếp đang bận
        if (hasUnCookFood)
            return;
        if(hasCookedFood) {
            GiveFood(VDGlobal.Instance.PlayerController);
            return;
        }
        if(sender==null) return;
        for (int i = 0; i < requireds.Count; i++)
        {
            if (sender.id == requireds[i].id)
            {
                isMakingIndex = i;
                VDGlobal.Instance.PlayerController.ClearObjectInHand();
                PutFood();
                break;
            }
        }
    }

    private void PutFood()
    {
        currentFood = Instantiate(
            requireds[isMakingIndex].prefab,
            transform.position,
            transform.rotation);
        hasUnCookFood = true;
        remainTime = cookTime;
    }

    private void OnCookDone()
    {
        hasCookedFood = true;
        hasUnCookFood = false;
        remainTime = cookTime;
        if (currentFood != null)
        {
            Destroy(currentFood);
        }
        currentFood = Instantiate(
            results[isMakingIndex].prefab,
            transform.position,
            transform.rotation);
    }

    public void RemoveCookedFood()
    {
        if (!hasCookedFood)
            return;
        if (currentFood != null)
        {
            Destroy(currentFood);
        }
        currentFood = null;
        hasCookedFood = false;
    }

    public void GiveFood(IObjectRecevier recevier)
    {
        RemoveCookedFood();
        if(isMakingIndex<0) return;
        recevier.Receive(transform,results[isMakingIndex]);
        isMakingIndex = -1;
    }
}