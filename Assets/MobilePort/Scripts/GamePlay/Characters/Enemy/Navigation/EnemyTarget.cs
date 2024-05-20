using CharacterBehaviour;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    [SerializeField] float randDistance = 10f;

    public EnemyStateManager ownerManager;
    public CharacterStateManager currentTargetManager;
    Transform ownerTransform;

    public float ownerDistance;
    public float ownerY;

    public List<Transform> nearPlayers;
    //public List<Transform> spottedPlayers;

    public float angle;

    public bool isTargetSpotted = false;
    void Start()
    {
        ownerTransform = ownerManager.transform;
        transform.SetParent(null);
    }


    void Update()
    {
        ownerDistance = Vector3.Distance(transform.position, ownerTransform.position);
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
        if(nearPlayers.Count > 0)
        {
            VisibilityAngleCalculation();
        }
            RandomTargetPosition();
        
    }
    void VisibilityAngleCalculation()
    {
        foreach (Transform t in nearPlayers)
        {
            Vector3 dir = (t.position - ownerTransform.position).normalized;
            angle = Vector3.Dot(ownerTransform.forward, dir);
            if(angle > 0.1f)
            {
                PlayerSpotted(t);
                return;
            }
        }
    }
    void RandomTargetPosition()
    {
        if (ownerDistance > 1f) return;
            transform.position = new Vector3(transform.position.x + Random.Range(-randDistance, randDistance), ownerY, transform.position.z + Random.Range(-randDistance, randDistance));
    }
    void PlayerSpotted(Transform target)
    {
        transform.SetParent(target, true);
        transform.localPosition = Vector3.zero+Vector3.up*2f;
        currentTargetManager = target.GetComponent<CharacterStateManager>();
        isTargetSpotted = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CharacterStateManager characterManager))
        {
            nearPlayers.Add(characterManager.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out CharacterStateManager characterManager))
        {
            nearPlayers.Remove(characterManager.transform);
        }
    }
}
