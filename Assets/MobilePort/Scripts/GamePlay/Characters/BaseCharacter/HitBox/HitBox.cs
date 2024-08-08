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
    public CharacterStateManager manager;
    //public GameObject bloodPs;

    Rigidbody rb;
    Rigidbody rbHb;
    PoolzSystem poolzSystem;
    Collider _collider;
    CharacterAnimationRiging rigs;
    Transform body;

    private void Awake()
    {
        rigs = GetComponentInParent<CharacterAnimationRiging>();    
        manager = GetComponentInParent<CharacterStateManager>();
        if(manager != null )
        rb = manager.GetComponent<Rigidbody>();

        _collider = GetComponent<Collider>();
    }
    private void Start()
    {
        if(manager!=null)
        {
            rbHb = GetComponent<Rigidbody>();
            Destroy(rbHb); 
        }

        poolzSystem = PoolzSystem.instance;
        if (manager)
            body = manager.body;
        else
            body = transform;
    }
  /*  public override void OnStartServer()
    {
        base.OnStartServer();
        if(IsServer)
        {
            rbHb = GetComponent<Rigidbody>();
            rbHb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }
  */
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
                
                //if (!other.CompareTag("Weapon")) return;
                //poolzSystem.Spawn(bloodPs,transform.position);
                //weapon.currentOpponent = manager;


                Vector3 hitPos = _collider.ClosestPointOnBounds(other.transform.position);
                Vector3 raycastDirection = (other.transform.position - hitPos);
                Vector3 hitForces = (hitPos - other.transform.position ).normalized;
                Vector3 hitForces2 = (other.transform.position - transform.position).normalized;
                Quaternion spawnRot = Quaternion.LookRotation(raycastDirection,Vector3.up);
                //spawnRot = other.transform.rotation;
                HitVfxObservers(null, hitPos, spawnRot, hitForces2, 20f, gameObject);
                if (rigs == null) return;

                rigs.SetBodyWeightObserver(1f, _bodyPart, 0f);
                rigs.SetRigWeightObserver(1f, RigPart.hit, .3f);
                rigs.FollowSourcePositionObservers(other.transform/*weapon.transform*/, rigs.hitSource, 2f);

                // poolzSystem.Spawn(poolzSystem.poolz[0].prefab, hitPos, spawnRot);
                // poolzSystem.Spawn(poolzSystem.poolz[1].prefab, hitPos, spawnRot);
                // int random = Random.Range(1, 60);
                // sceneController.DmgText(random.ToString());
            }

            //  poolzSystem.SpawnNobServer(poolzSystem.poolz[0].prefab,transform.position,spawnRot);
            //poolzSystem.SpawnNobServer(poolzSystem.poolz[1].prefab,transform.position,spawnRot);

            // manager.getHitState.Weapon = weapon.transform;
        }
    }

    [ObserversRpc]
    void HitVfxObservers(Weapon weapon, Vector3 source, Quaternion rot, Vector3 dir ,float dmg, GameObject go)
    {
        PoolzSystem.instance.Spawn(poolzSystem.poolz[0].prefab, source, rot, go,null);
        // poolzSystem.Spawn(poolzSystem.poolz[1].prefab, source, rot, transform);
        //  poolzSystem.Spawn(poolzSystem.poolz[3].prefab, source, rot, transform);
        //poolzSystem.Spawn(poolzSystem.poolz[4].prefab, source, rot, transform);

        //sceneController.DmgText(dmg.ToString());
        ///weapon.isHBDetected = true;
        //   manager.getHitState.Weapon = weapon.transform;
        // manager.getHitState.bodyPartHit = _bodyPart;
        if (manager == null) return;

        manager.SwitchCurrentState(manager.getHitState);
        rb.AddForce(dir * 2, ForceMode.Impulse);
        manager.myHealth.UpdateHealth(-20);
    }
}
