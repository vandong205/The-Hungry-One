using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum IngredientID
{
    Cheese,
    Basil,
    PineApple,
    Pepper,
    Mushroom,
    Ham,
    Pepperoni
}

public class IngredientTable : MonoBehaviour
{
    [SerializeField] List<IngredientStray> strays;
    [SerializeField] DoughPlate doughPlate;

    [SerializeField] float ingredientYOffset = 0.5f;
    [SerializeField] float flyDuration = 0.5f;

    void Awake()
    {
        foreach (IngredientStray stray in strays)
        {
            stray.OnStrayPicked += OnIngredentPick;

        }
    }

    private void OnIngredentPick(IngredientID ingredientID,Transform sender,GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"No prefab assigned for {ingredientID}");
            return;
        }
        if(!doughPlate.hasDough) return;

        GameObject flyingIngredient =
            Instantiate(prefab, sender.position, sender.rotation);

        Vector3 targetPos =
            doughPlate.transform.position +
            Vector3.up * ingredientYOffset;

        flyingIngredient.transform
            .DOMove(targetPos, flyDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                doughPlate.ReceiveIngredient(ingredientID);

                Destroy(flyingIngredient);
            });
    }
}