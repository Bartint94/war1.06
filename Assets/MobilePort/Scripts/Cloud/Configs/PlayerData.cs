using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Save System")]
public class PlayerData : ScriptableObject
{
    public string playerName;
    public Texture[] baseSkin;
    public int currentBaseSkinId;
    public Weapon[] weapons;
    public int currentWeaponId;
    public Arrows[] arrows;
    public int currentArrowId;
    public ProtectionItemConfig[] headProtections;
    public int currentHeadProtectionId;
    public ProtectionItemConfig[] torsoProtections;
    public int currentTorsoProtectionId;
    public ProtectionItemConfig[] legsProtections;
    public int currentLegsProtectionId;
    public ProtectionItemConfig[] feetProtections;
    public int currentFeetProtectionId;


    
}
