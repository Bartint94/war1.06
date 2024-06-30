using CharacterBehaviour;
using FishNet.Component.Prediction;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Arrows : MonoBehaviour, ISpawnable, IOffensive
{
    Rigidbody _rigidbody;
    Weapon _weapon;
    Collider _collider;
    [SerializeField] float force = 20f;
    GameObject _owner;
    bool isFly;
    Inventory inventory;
    CharacterStateManager manager => inventory.characterStateManager;

    private void Awake()
    {
    }
    public void Init(Vector3 position, Quaternion rotation, GameObject owner, Inventory inventory = null)
    {
        // transform.position = position;
        // transform.rotation = rotation;
        // _rigidbody = GetComponent<Rigidbody>();
          // _rigidbody.isKinematic = true;
       // if (IsServer)
        {
            // _weapon = GetComponent<Weapon>();
            // _weapon = GetComponent<Weapon>();
            // _weapon.manager = owner.GetComponent<CharacterStateManager>(); 
            this.inventory = inventory;
            transform.SetParent(inventory.aim, false);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
    }
 //   [ServerRpc(RequireOwnership = false)]
    public void Shot(Vector3 pos, Quaternion rot)
    {
        // if (!IsServer) return;
    
      //  transform.SetParent(null, true);
        ShotObserver(pos, rot);
    }
    
    public void ShotObserver(Vector3 pos, Quaternion rot)
    {
        transform.SetParent(null , true);
        //_rigidbody.isKinematic = false;
       // transform.position = pos;
        //transform.rotation = rot;
        //_rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
        isFly = true;
    }
    private void FixedUpdate()
    {
        if(isFly)
        transform.Translate(transform.forward *  force * Time.deltaTime, Space.World);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
          if (other.TryGetComponent(out HitBox hitBox))
          {
            if (CheckTarget(hitBox.manager))
              isFly = false;
                 // _rigidbody.isKinematic = true;
                 // transform.parent  = other.transform;
                  //transform.localRotation = Quaternion.Euler(0, 0, 0);
              
          }
       
        
    }

    public bool CheckTarget(CharacterStateManager manager)
    {
        if (manager == this.manager)
        {
            return false;
        }
        else
            return true;
    }
}
