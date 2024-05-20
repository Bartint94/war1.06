using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeUI : UiButtons
{
    CloudSaveData cloudSaveData;
    [SerializeField] PlayerData playerData;
    [SerializeField] SkinnedMeshRenderer baseSkin;


    public int baseTexId;
    public readonly string baseTexKey = "baseTex";
    private void Start()
    {
    }
    public void InitCategory()
    {
        InitButtons(playerData.currentBaseSkinId, playerData.baseSkin.Length);
    }
    public override void LeftButton()
    {
        base.LeftButton();
        baseSkin.materials[0].mainTexture = playerData.baseSkin[buttonId];

        playerData.currentBaseSkinId = buttonId;
/*
        if (baseTexId == 0)
        {
            leftButton.interactable = false;
        }
        if (!rightButton.interactable)
        {
            rightButton.interactable = true;
        }
  */
    }
    public override void RightButton()
    {
        base.RightButton();
 
        baseSkin.materials[0].mainTexture = playerData.baseSkin[buttonId];

        playerData.currentBaseSkinId = buttonId;
/*
        if (baseTexId == playerData.baseSkin.Length - 1)
        {
            rightButton.interactable = false;
        }
        if (!leftButton.interactable)
        {
            leftButton.interactable = true;
        }
  */
    }

    public async Task LoadCustomization()
    {
        cloudSaveData = FindAnyObjectByType<CloudSaveData>();
        int reasult = await cloudSaveData.LoadData(baseTexKey);
        baseTexId = reasult;

        baseSkin.materials[0].mainTexture = playerData.baseSkin[baseTexId];

        playerData.currentBaseSkinId = baseTexId;

        //InitButtons(baseTexId, playerData.baseSkin.Length);
    }
    public void SaveCustomization()
    {
        cloudSaveData.SaveData(baseTexKey,buttonId);
    }

}
