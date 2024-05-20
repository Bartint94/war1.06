using CharacterBehaviour;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrows : NetworkBehaviour, ISpawnable
{
    Rigidbody _rigidbody;
    Weapon _weapon;
    Collider _collider;
    [SerializeField] float force = 20f;
    public void Init(Vector3 position, Quaternion rotation, Transform owner, Inventory inventory = null)
    {
        transform.position = position;
        transform.rotation = rotation;
        _weapon = GetComponent<Weapon>();
        _weapon.manager = owner.GetComponent<CharacterStateManager>();
        if (!IsServer) return;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
    }


    void Start()
    {
        
    }

    void UpdateServer()
    {

    }
    void UpdateObserver()
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if (other.TryGetComponent(out HitBox hitBox))
        {
            if (_weapon.manager.CheckHitValidation(_weapon))
            {
                _rigidbody.isKinematic = true;
                transform.SetParent(other.transform);
            }
        }
      
    }
}
