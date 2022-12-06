using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTile : MonoBehaviour
{

    private Crop curCrop;
    public GameObject cropPrefab;

    public SpriteRenderer sr;
    private bool tilled;

    [Header("Sprites")]
    public Sprite grassSprite;
    public Sprite tilledSprite;
    public Sprite waterTilledSprite;

    void Start()
    {
        // Set the default grass sprite;
        sr.sprite = grassSprite;
    }
    //Tile allows crop to be planted.
    //Gives the ability to the player to till,plant crop, water crop, and harvest crop.
    public void Interact ()
    {
        if(!tilled)
        {
            Till();
        }
        else if (!HasCrop() && GameManager.instance.CanPlantCrop())
        {
            PlantNewCrop(GameManager.instance.WheatCropToPlant);
        }
       
        else if(HasCrop() && curCrop.CanHarvest())
        {
            curCrop.Harvest();
        }
        else
        {
            Water();
        }
    }

    void PlantNewCrop (CropData crop)
    {
        if (!tilled)
            return;

        curCrop = Instantiate(cropPrefab, transform).GetComponent<Crop>();
        curCrop.Plant(crop);

        GameManager.instance.onNewMonth += OnNewMonth;

    }
    void Till ()
    {
        tilled = true;
        sr.sprite = tilledSprite;
    }

    void Water ()
    {
        sr.sprite = waterTilledSprite;

        if(HasCrop())
        {
            curCrop.Water();
        }
    }

    void OnNewMonth ()
    {
        if(curCrop == null)
        {
            tilled = false;
            sr.sprite = grassSprite;

            GameManager.instance.onNewMonth -= OnNewMonth;
        }
        else if(curCrop != null)
        {
            sr.sprite = tilledSprite;
            curCrop.NewMonthCheck();
        }
              
    }

    bool HasCrop ()
    {
        return curCrop != null;
    }
}
