using CharacterBehaviour;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState { attack, deffence }
public interface IOffensive
{
    public float Dmg();
    public bool IsValidatedHit(CharacterManager manager);
    public void GetManager(out CharacterManager manager);
    
}
public class Weapon : Item, IOffensive
{
    Collider _collider;

    public CharacterManager manager;
    public PlayerAttackState attackState;

    public CharacterManager currentOpponent;

    public bool isHBDetected;


    [SerializeField] bool isRight;
    [SerializeField] bool isDistanceFighting;

    [SerializeField] Vector3 holdPosition;
    [SerializeField] Vector3 holdRotation;

    [SerializeField] RuntimeAnimatorController controller;
    public ParticleSystem vfx;

    [SerializeField] float _dmg;
    public float dmg => _dmg;

   


    Vector3 _currentSource;
    public Vector3 currentSource => _currentSource;

    public WeaponState currentWeaponState;

    public List<GameObject> hitedObjects;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public override void Init(Inventory inventory)
    {
        base.Init(inventory);

        inventory.rigging.InitAimingStyle(isDistanceFighting);
        manager = inventory.characterStateManager;
        inventory.currentWeapon = this;
        inventory.blockDetector.myWeapon = this;

        if (manager != null)
        {
            manager.isDistanceFighting = isDistanceFighting;
        }

        if (isRight)
        {
            //attackState = inventory.attackState;
            inventory.itemHolders[1].item = this;
            inventory.itemHolders[1].isEquiped = true;

            transform.SetParent(inventory.itemHolders[1].transform, true);
            transform.localPosition = holdPosition;
            transform.localEulerAngles = holdRotation;

            inventory.ChangeAnimator(controller);

        }
        else
        {
            inventory.itemHolders[0].item = this;
            inventory.itemHolders[0].isEquiped = true;

            transform.SetParent(inventory.itemHolders[0].transform, true);
            transform.localPosition = holdPosition;
            transform.localEulerAngles = holdRotation;

            inventory.ChangeAnimator(controller);
        }
    }
    public void ToggleWeaponTrigger(bool trigger, Vector3 source, WeaponState weaponState)
    {
        currentWeaponState = weaponState;
        _collider.enabled = trigger;
        _currentSource = source;
        currentOpponent = null;
        if (trigger)
        {
            CheckHitedReset();
        }
    }
  
    [ObserversRpc]
    void CheckHitedTest(GameObject other)
    {
        hitedObjects.Add(other);

    }
    [ObserversRpc]
    void CheckHitedReset()
    {
        hitedObjects.Clear();
    }

    public bool IsValidatedHit(CharacterManager target)
    {
        if(target == manager)
        {
            return false;
        }
      
        if (currentOpponent == target)
        {
            return false;
        }
        if (currentWeaponState == WeaponState.deffence)
        {
            return false;
        }
        currentOpponent = target;
        return true;
        
    }

    public float Dmg()
    {
        return dmg;
    }

    public void GetManager(out CharacterManager manager)
    {
        manager = this.manager;
    }
}



