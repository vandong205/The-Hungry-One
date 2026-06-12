using System.Collections.Generic;
using UnityEngine;

public class StoveCookSurface : MonoBehaviour, IInteractableObject
{
    [SerializeField] List<ObjectData> requireds;
    [SerializeField] List<ObjectData> results;

    [SerializeField] float cookTime = 10f;

    [Header("Cook Done Bounce")]
    [SerializeField] float bounceUpForce = 3f;
    [SerializeField] float bounceRandomForce = 1f;

    private bool isBusy=false;
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
        if (!isBusy||!isOn)
            return;
        remainTime -= Time.deltaTime;
        if (remainTime <= 0)
        {
            OnCookDone();
        }
    }

    public void Interact(string sender)
    {
        // Bếp đang bận
        if (isBusy)
            return;
        if(hasCookedFood) {
            GiveFood(VDGlobal.Instance.PlayerController);
            return;
        }
        for (int i = 0; i < requireds.Count; i++)
        {
            if (sender == requireds[i].id)
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

        isBusy = true;
        remainTime = cookTime;
    }

    private void OnCookDone()
    {
        isBusy = false;
        hasCookedFood = true;
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

    public void TakeFood()
    {
        if (!hasCookedFood)
            return;
        Destroy(currentFood);
        currentFood = null;
        hasCookedFood = false;
    }

    public void GiveFood(IObjectRecevier recevier)
    {
        TakeFood();
        recevier.Receive(transform,results[isMakingIndex]);
        isMakingIndex = -1;
    }
}