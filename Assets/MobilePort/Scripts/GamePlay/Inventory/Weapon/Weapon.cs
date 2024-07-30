using CharacterBehaviour;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponState { attack, deffence }
public interface IOffensive
{
    public bool CheckTarget(CharacterStateManager manager);
}
public class Weapon : Item, IOffensive
{
    Collider _collider;

    public CharacterStateManager manager;
    public PlayerAttackState attackState;

    public CharacterStateManager currentOpponent;

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
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) { return; }
        if (other.TryGetComponent(out Weapon opponrntWeapon))
        {
            currentOpponent = opponrntWeapon.manager;
            //attackState.AttackBlocked();
            //      CheckHitedTest(other.gameObject);
        }
        if (other.TryGetComponent(out HitBox hitBox))
        {
            CheckHitedTest(other.gameObject);

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

    public bool CheckTarget(CharacterStateManager manger)
    {
        if(manger == this.manager)
        {
            return false;
        }
      
        if (currentOpponent == this.manager)
        {
            return false;
        }
        if (currentWeaponState == WeaponState.deffence)
        {
            return false;
        }
        
        return true;
        
    }
}



