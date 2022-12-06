using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{
    private CropData curCrop;
    private int plantMonth;
    private int monthsSinceLastWatered;

    public SpriteRenderer sr;

    public static event UnityAction<CropData> onPlantCrop;
    public static event UnityAction<CropData> onHarvestCrop;

    public void Plant (CropData crop)
    {
        curCrop = crop;
        plantMonth = GameManager.instance.curMonth;
        monthsSinceLastWatered = 1;
        UpdateCropSprite();

        onPlantCrop?.Invoke(crop);
    }

    public void NewMonthCheck ()
    {
        monthsSinceLastWatered++;

        if(monthsSinceLastWatered > 3)
        {
            Destroy(gameObject);
        }

        UpdateCropSprite();
    }

    void UpdateCropSprite ()
    {
        int cropProg = CropProgress();

        if(cropProg < curCrop.monthsToGrow)
        {
            sr.sprite = curCrop.growProgressSprites[cropProg];
        }
        else
        {
            sr.sprite = curCrop.readyToHarvestSprite;
        }
    }

    public void Water ()
    {
        monthsSinceLastWatered = 0;
    }

    public void Harvest ()
    {
        if(CanHarvest())
        {
            onHarvestCrop?.Invoke(curCrop);
            Destroy(gameObject);
        }
                
    }

    int CropProgress ()
    {
        return GameManager.instance.curMonth - plantMonth;
    }

    public bool CanHarvest ()
    {
        return CropProgress() >= curCrop.monthsToGrow; 
    }

}
