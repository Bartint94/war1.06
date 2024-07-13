using CharacterBehaviour;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public enum ItemCategory {Weapon, Head, Torso, Legs, Feet}
public class Inventory : NetworkBehaviour
{
    [SerializeField] PlayerData playerData;
    [SerializeField] SkinnedMeshRenderer headRenderer;
    [SerializeField] SkinnedMeshRenderer torsoRenderer;
    [SerializeField] SkinnedMeshRenderer legsRenderer;
    [SerializeField] SkinnedMeshRenderer feetRenderer;

    public ItemHolder[] itemHolders;

    public CharacterStateManager characterStateManager;
    public CharacterState characterState;
    public Weapon currentWeapon;
    public BlockDetector blockDetector;
    public CharacterAnimationRiging rigging;
    public Transform aim => distanceAttack.aim;

    Animator animator;
    PlayerDistanceAttackState distanceAttack;

   // public PlayerAttackState attackState;


    private void Awake()
    {
       // attackState = GetComponent<PlayerAttackState>();
        distanceAttack = GetComponent<PlayerDistanceAttackState>();
        itemHolders = GetComponentsInChildren<ItemHolder>();
        characterStateManager = GetComponent<CharacterStateManager>();
        animator = GetComponent<Animator>();
        blockDetector = GetComponentInChildren<BlockDetector>();
        rigging = GetComponent<CharacterAnimationRiging>();
        
    }
    public void ChangeAnimator(RuntimeAnimatorController controller)
    {
        animator.runtimeAnimatorController = controller;
    }
    public void SetAim(Transform aim)
    {
        if(distanceAttack == null) { return; }
        distanceAttack.aim = aim;   
    }
    [ServerRpc]
    public void RemoveWeaponsNob()
    {
        foreach (var item in itemHolders)
        {
            if(item.isEquiped)
            {
                base.Despawn(item.item.NetworkObject);
                item.isEquiped = false;
            }
        }
    }

    void RemoveWeapons()
    {
        foreach (var item in itemHolders)
        {
            if (item.isEquiped)
            {
                Destroy(item.item.gameObject);
                item.isEquiped = false;
            }
        }
    }


    public void InitItem(int itemId, ItemCategory category, bool isNob = true)
    {
        if(category == ItemCategory.Weapon)
        {
            InitWeapon(itemId,isNob);
        }
        if(category == ItemCategory.Torso)
        {
            InitTorso(itemId);
        }
        if(category == ItemCategory.Head)
        {
            InitHead(itemId);
        }
        if(category == ItemCategory.Legs)
        {
            InitLegs(itemId);
        }
        if(category == ItemCategory.Feet)
        {
            InitFeet(itemId);
        }
    }
    void InitWeapon(int weaponId, bool isNob)
    {
        
        if (isNob)
        {
            RemoveWeapons();
            PoolzSystem.instance.SpawnNobServer(playerData.weapons[weaponId].gameObject, Vector3.zero, Quaternion.identity, null, this);
        }
        else
        {
            RemoveWeapons();
            var weapon = PoolzSystem.instance.SpawnNob(playerData.weapons[weaponId].gameObject, Vector3.zero, Quaternion.identity,null,this);
            Destroy(weapon.GetComponent<NetworkObject>());
            weapon.gameObject.SetActive(true);
            //weapon.Init(this);
        }
    }
    void InitTorso(int itemId)
    {
        torsoRenderer.sharedMesh = playerData.torsoProtections[itemId]._Mesh;
    }
    void InitHead(int itemId)
    {
         headRenderer.sharedMesh = playerData.headProtections[itemId]._Mesh;
    }
    void InitLegs(int itemId)
    {
        legsRenderer.sharedMesh = playerData.legsProtections[itemId]._Mesh;
    }
    void InitFeet(int itemId)
    {
        feetRenderer.sharedMesh = playerData.feetProtections[itemId]._Mesh;
    }



    [ServerRpc]
    public void WeaponTriggerToggleServer(bool turn,WeaponState wepaonState)
    {
        if (characterStateManager.currentState == characterStateManager.getHitState)
        {
            return;
        }
        currentWeapon.ToggleWeaponTrigger(turn, currentWeapon.transform.position, wepaonState);
    }
   /* [ObserversRpc(BufferLast = true)]
    public void WeaponTriggerToggleObserver(bool turn, Vector3 source, WeaponState wepaonState = WeaponState.attack)
    {

        currentWeapon.ToggleWeaponTrigger(turn, source, wepaonState);
    }
   */
    public void WeaponTriggerToggle(bool turn, WeaponState wepaonState)
    {
        if (characterStateManager.currentState == characterStateManager.getHitState)
        {
            return;
        }
        currentWeapon.ToggleWeaponTrigger(turn,currentWeapon.transform.position, wepaonState );
    }

    /*
        public override void OnStartClient()
        {
            base.OnStartClient();
            InitializeInventory();
        }



        void InitializeInventory()
        {
            foreach (var item in itemHolders)
            {
                item.SetupRig();
            }
        }



        private void Update()
        {
            if (!IsOwner) return;
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                LeftArmItemToggleServer();
            }
        }
        [ServerRpc]
        void LeftArmItemToggleServer()
        {
            LeftArmItemToggleObserver();
        }
        [ObserversRpc(BufferLast = true)]
        void LeftArmItemToggleObserver()
        {
            leftArmHolder.SwitchItem();
        }
    */
}
