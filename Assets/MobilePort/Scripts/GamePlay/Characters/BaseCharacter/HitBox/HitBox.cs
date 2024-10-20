using CharacterBehaviour;
using FishNet.Managing.Scened;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum BodyPart {LegLeft,LegRight,Spin,ArmLeft,ArmRight,Head};
public class HitBox : NetworkBehaviour
{
    public BodyPart _bodyPart;
    public CharacterManager manager;
    Inventory inventory;
    //public GameObject bloodPs;

   // Rigidbody rb;
   // Rigidbody rbHb;
    PoolzSystem poolzSystem;
    Collider _collider;
    CharacterAnimationRiging rigs;
    Transform body;



    Vector3 hitSource;
    Vector3 hitDirection;
    Vector3 hitDirectionNormalized;
    Quaternion spawnRot;
    private void Awake()
    {
        rigs = GetComponentInParent<CharacterAnimationRiging>();    
        inventory = GetComponentInParent<Inventory>();
        manager = GetComponentInParent<CharacterManager>();
        if(manager != null )
       // rb = manager.GetComponent<Rigidbody>();

        _collider = GetComponent<Collider>();
    }
    private void Start()
    {
        if(manager!=null)
        {
            //rbHb = GetComponent<Rigidbody>();
           // Destroy(rbHb); 
        }

        poolzSystem = PoolzSystem.instance;
        if (manager)
            body = manager.body;
        else
            body = transform;
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IOffensive weapon))
        {
           
            if (IsServer)
            {
                if (!weapon.IsValidatedHit(manager))
                {
                    return;
                }

                LockWeapon();

                hitSource = _collider.ClosestPointOnBounds(other.transform.position);


                hitDirection = (other.transform.position - hitSource);
                spawnRot = Quaternion.LookRotation(hitDirection, Vector3.up);

                CharacterManager weaponManager = null;
                weapon.GetManager(out weaponManager);
                hitDirection += (transform.position - weaponManager.transform.position).normalized;
                if(hitDirectionNormalized != Vector3.zero)
                {
                    hitDirectionNormalized = new Vector3(hitDirection.x, 0f, hitDirection.z);

                }
                else
                {
                    hitDirectionNormalized = new Vector3(Random.Range(-1f,1f), 0f, Random.Range(-1f, 1f));
                    Debug.Log("RandomDirection");
                }
                hitDirectionNormalized.Normalize();

                HitVfxObservers(hitSource, hitDirectionNormalized, spawnRot, weapon.Dmg(), gameObject);
            }

        }
    }

    private void LockWeapon()
    {
        inventory.WeaponTriggerToggle(false, WeaponState.deffence);
    }

    [ObserversRpc]
    void HitVfxObservers(Vector3 source, Vector3 dir, Quaternion rot, float dmg, GameObject go)
    {
        PoolzSystem.instance.Spawn(poolzSystem.poolz[0].prefab, source, rot, go,null);
 
        if (manager == null) return;

        manager._rigidbody.AddForce(dir * dmg, ForceMode.Impulse);

        

        if (IsOwner || manager.isBot)
        {
            //manager.getHitState.direction = dir;
            manager.getHitState.hitPosition = source;
            manager.getHitState.bodyPartHit.Add(_bodyPart);

            manager.SwitchCurrentState(manager.getHitState);
        }

        //rb.AddForce(dir * 15f + Vector3.up* 2f, ForceMode.Impulse);
        manager.myHealth.UpdateHealth(-20);
    }
}
