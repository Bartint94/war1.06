using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterLoad : NetworkBehaviour
{
    [SerializeField] SkinnedMeshRenderer baseSkin;
    [SerializeField] PlayerData playerData;

    [SerializeField] Inventory inventory;
    [SerializeField] TextMeshProUGUI _playerName;
    [SerializeField] bool isNpc;
    private void Awake()
    {
       // inventory = GetComponent<Inventory>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient(); 
        if(IsOwner)
        {
            LoadCustomizeServer(playerData.playerName, playerData.currentBaseSkinId, playerData.currentWeaponId, playerData.currentHeadProtectionId ,playerData.currentTorsoProtectionId, playerData.currentLegsProtectionId, playerData.currentFeetProtectionId);
            inventory.InitItem(playerData.currentWeaponId,ItemCategory.Weapon);
        }
        if(IsServer)
        {
            if(isNpc)
            {
                inventory.InitItem(playerData.currentWeaponId, ItemCategory.Weapon);
            }
        }
    }
   

    [ServerRpc]
    void LoadCustomizeServer(string name, int baseId, int weaponId,int headId, int torsoId, int legsId,int feetId)
    {

        LoadCustomizeObserver(name, baseId, headId, torsoId, legsId, feetId);
       // inventory.InitItem(weaponId,ItemCategory.Weapon);
    }
    [ObserversRpc(BufferLast = true)]
    void LoadCustomizeObserver(string name, int baseId ,int headId, int torsoId, int legsId, int feetId)
    {
        baseSkin.materials[0].SetTexture("_BaseMap", playerData.baseSkin[baseId]);

        inventory.InitItem(headId, ItemCategory.Head, false);
        inventory.InitItem(torsoId, ItemCategory.Torso, false);
        inventory.InitItem(legsId, ItemCategory.Legs, false);
        inventory.InitItem(feetId, ItemCategory.Feet, false);
        _playerName.text = name;
        // inventory.RemoveItems();
    }



}
