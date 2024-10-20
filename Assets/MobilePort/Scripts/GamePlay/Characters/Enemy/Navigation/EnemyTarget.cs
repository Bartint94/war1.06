using CharacterBehaviour;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTarget : NetworkBehaviour
{
    [SerializeField] float randDistance = 10f;

    public EnemyStateManager ownerManager;
    Transform ownerTransform;
    public CharacterManager currentTargetManager;

    public float distance;
    public float ownerY;

    public List<CharacterManager> nearTargets;
    //public List<Transform> spottedPlayers;

    public float angle;

    public bool isTargetSpotted = false;
    public override void OnStartServer()
    {
        base.OnStartServer();
    
    
        if (!IsServer) return;
        ownerTransform = ownerManager.transform;
        transform.SetParent(null);
    }


    void Update()
    {
        if (!IsServer) return;

        distance = Vector3.Distance(transform.position, ownerTransform.position);
        ownerY = ownerTransform.position.y;
        if(isTargetSpotted)
        {
            return;
        }
        else
        {
            if (transform.position.y != ownerY)
                transform.position = new Vector3(transform.position.x, ownerY, transform.position.z);
        }
        if(nearTargets.Count > 0)
        {
            VisibilityAngleCalculation();
        }
            RandomTargetPosition();
        
    }
    void VisibilityAngleCalculation()
    {
        foreach (var target in nearTargets)
        {
            Vector3 dir = (target.transform.position - ownerTransform.position).normalized;
            angle = Vector3.Dot(ownerTransform.forward, dir);
            if(angle > 0.1f)
            {
                PlayerSpotted(target.transform);
                return;
            }
        }
    }
    void RandomTargetPosition()
    {
        if (distance > 1f) return;
            transform.position = new Vector3(transform.position.x + Random.Range(-randDistance, randDistance), ownerY, transform.position.z + Random.Range(-randDistance, randDistance));
    }
    void PlayerSpotted(Transform target)
    {
        transform.SetParent(target, true);
        transform.localPosition = Vector3.zero + Vector3.up*2f;
        currentTargetManager = target.GetComponent<CharacterManager>();
        isTargetSpotted = true;
    }

    public void RemoceCurrentTarget()
    {
        nearTargets.Remove(currentTargetManager);
        isTargetSpotted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterManager characterManager))
        {
            if (!characterManager.isDead && characterManager != ownerManager)

                nearTargets.Add(characterManager);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterManager characterManager))
        {
            nearTargets.Remove(characterManager);
        }
    }
}
