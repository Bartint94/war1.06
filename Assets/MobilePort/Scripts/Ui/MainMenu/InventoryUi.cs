using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUi : UiButtons
{
    CloudSaveData cloudSaveData;

    public ItemCategory currentCategory;


    public readonly string wepaonKey = "wepaon";
    public readonly string headKey = "head";
    public readonly string torsoKey = "torso";
    public readonly string legsKey = "legs";
    public readonly string feetKey = "feet";

    string cloudKey;

    [SerializeField] PlayerData playerData;
    [SerializeField] Inventory inventory;

    [SerializeField] GameObject buttonsWindow;

    [SerializeField] Button armorButton;
    [SerializeField] Button legsButton;
    [SerializeField] Button bootsButton;
    private void Start()
    {
        cloudSaveData = FindAnyObjectByType<CloudSaveData>();
    }

    public void InitCategory(int category)
    {
        buttonsWindow.SetActive(true);

        currentCategory = (ItemCategory)category;

        if(currentCategory == ItemCategory.Weapon)
        {
            cloudKey = wepaonKey;

            InitButtons(playerData.currentWeaponId,playerData.weapons.Length);  
        }
        if(currentCategory == ItemCategory.Head)
        {
            cloudKey = headKey;

            InitButtons(playerData.currentHeadProtectionId, playerData.headProtections.Length);
        }
        if(currentCategory==ItemCategory.Torso)
        {
            cloudKey = torsoKey;

            InitButtons(playerData.currentTorsoProtectionId, playerData.torsoProtections.Length);
        }
        if(currentCategory == ItemCategory.Legs)
        {
            cloudKey = legsKey;

            InitButtons(playerData.currentLegsProtectionId, playerData.legsProtections.Length);
        }
        if(currentCategory == ItemCategory.Feet)
        {
            cloudKey = feetKey;

            InitButtons(playerData.currentFeetProtectionId, playerData.feetProtections.Length);
        }
    }
    public override void RightButton()
    {
        base.RightButton();

        inventory.InitItem(buttonId, currentCategory, false); 
    }
    public override void LeftButton()
    {
        base.LeftButton();

        inventory.InitItem(buttonId, currentCategory, false);

    }
    public async Task LoadInventory()
    {
        int weapon = await cloudSaveData.LoadData(wepaonKey);

        inventory.InitItem(weapon,ItemCategory.Weapon,false);
        playerData.currentWeaponId = weapon;


        int head = await cloudSaveData.LoadData(headKey);

        inventory.InitItem(head,ItemCategory.Head,false);
        playerData.currentHeadProtectionId = head;


        int torso = await cloudSaveData.LoadData(torsoKey);

        inventory.InitItem(torso,ItemCategory.Torso,false);
        playerData.currentTorsoProtectionId = torso;


        int legs = await cloudSaveData.LoadData(legsKey);

        inventory.InitItem(legs,ItemCategory.Legs,false);
        playerData.currentLegsProtectionId = legs;


        int feet = await cloudSaveData.LoadData(feetKey);

        inventory.InitItem(feet,ItemCategory.Feet,false);
        playerData.currentFeetProtectionId = feet;
    }
    public void SaveCustomization()
    {
        cloudSaveData.SaveData(cloudKey, buttonId);

        if(cloudKey == wepaonKey)
        {
            playerData.currentWeaponId = buttonId;
        }

        if(cloudKey == headKey) 
        {
            playerData.currentHeadProtectionId = buttonId;
        }

        if(cloudKey == torsoKey)
        {
            playerData.currentTorsoProtectionId = buttonId;
        }

        if(cloudKey == legsKey)
        {
            playerData.currentLegsProtectionId = buttonId;
        }

        if (cloudKey == feetKey)
        {
            playerData.currentFeetProtectionId = buttonId;
        }
    }
}
