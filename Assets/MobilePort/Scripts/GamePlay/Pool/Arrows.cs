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
    public bool isFly;
    Inventory inventory;
    CharacterStateManager manager => inventory.characterStateManager;


    [SerializeField] AnimationCurve flyCurve;
    [SerializeField] AnimationCurve gravCurve;

    Vector3 lastPos;
    Vector3 forwardDir;
    float duration;
    Vector3 speed;
    public float mag;
    [SerializeField] float lerp;
    [SerializeField] Transform visibleObj;
    [SerializeField] MiscOffset offset;
    
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
            transform.SetParent(inventory.itemHolders[1].transform, false);
            transform.localPosition = offset.position;
            transform.localRotation = offset.rotation;
        }
    }

    void SetAim()
    {
        transform.SetParent(inventory.aim, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
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
        SetAim();
        transform.SetParent(null , true);
        //_rigidbody.isKinematic = false;
        // transform.position = pos;
        //transform.rotation = rot;
        //_rigidbody.AddForce(transform.forward * force, ForceMode.Impulse);
      

        duration = 0;
     
        isFly = true;
    }
    float test;
    private void FixedUpdate()
    {
        if(isFly)

        {
            
            duration += Time.deltaTime;
            test = Mathf.Lerp(test, 1f, lerp *Time.deltaTime);
            var curve = flyCurve.Evaluate(test);
            var curveGrav = gravCurve.Evaluate(test);
           
            transform.Translate((transform.forward *  curve )+ (-Vector3.up * curveGrav), Space.World);
            VisibleObjCalculate();
            //transform.position = Vector3.Lerp(forwardDir, gravityDir, speed);
        }
        //transform.position = Vector3.Lerp()
        
    }
   
    void VisibleObjCalculate()
    {
        if (lastPos != transform.position)
        {
            speed = transform.position - lastPos;
            forwardDir = speed;
            speed /= Time.deltaTime;
            
            mag = speed.magnitude;
            lastPos = transform.position;
        }
        visibleObj.transform.rotation = Quaternion.LookRotation(forwardDir);
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
