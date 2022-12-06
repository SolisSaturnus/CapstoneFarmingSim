using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int curMonth;
    public int money;
    public int WheatInventory;
    public CropData WheatCropToPlant;
    public TextMeshProUGUI statsText;

    public event UnityAction onNewMonth;

    public static GameManager instance;

    void OnEnable()
    {
        Crop.onPlantCrop += OnPlantCrop;
        Crop.onHarvestCrop += OnHarvestCrop;
    }

    void OnDisable ()
    {
        Crop.onPlantCrop -= OnPlantCrop;
        Crop.onHarvestCrop -= OnHarvestCrop;
    }

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void SetNextMonth ()
    {
        curMonth++;
        onNewMonth?.Invoke();
        UpdateStatsText();
    }

    public void OnPlantCrop (CropData crop)
    {
        WheatInventory--;
        UpdateStatsText();
    }

    public void OnHarvestCrop(CropData crop)
    {
         money += crop.sellPrice;
         UpdateStatsText();
    }

    public void PurchaseCrop (CropData crop)
    {
        money -= crop.purchasePrice;
        WheatInventory++;
        UpdateStatsText();

        money -= crop.purchasePrice;
    }

    public bool CanPlantCrop ()
    {
        return WheatInventory > 0;
    }
    public void OnBuyCropButton (CropData crop)
    {
        if(money >= crop.purchasePrice)
        {
            PurchaseCrop(crop);
        }
    }
    void UpdateStatsText ()
    {
        statsText.text = $"Month: {curMonth}\nMoney: ${money}\nSeed Inventory: {WheatInventory}";
      
    }
}
