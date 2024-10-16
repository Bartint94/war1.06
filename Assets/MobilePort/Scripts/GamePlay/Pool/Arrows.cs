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
    ProjectileTrigger _projectileTrigger;
    [SerializeField] float force = 20f;
    GameObject _owner;
    public bool isFly;
    Inventory inventory;
    CharacterManager manager => inventory.characterStateManager;


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
    [SerializeField] float dmg;

    private void Awake()
    {
        _projectileTrigger = GetComponentInChildren<ProjectileTrigger>();
    }
    public void Init(Vector3 position, Quaternion rotation, GameObject owner, Inventory inventory = null)
    {
        this.inventory = inventory;
        transform.SetParent(inventory.itemHolders[1].transform, false);
        transform.localPosition = offset.position;
        transform.localRotation = offset.rotation;

    }

    void SetAim()
    {
        transform.SetParent(inventory.aim, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Shot(Vector3 pos, Quaternion rot)
    {

        ShotObserver(pos, rot);
    }
    
    public void ShotObserver(Vector3 pos, Quaternion rot)
    {
        _projectileTrigger.ActivateDetection();
        SetAim();
        transform.SetParent(null);

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

        }

        
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
            if (IsValidatedHit(hitBox.manager))
              isFly = false;
          }
       
        
    }

    public bool IsValidatedHit(CharacterManager manager)
    {
        if (manager == this.manager)
        {
            return false;
        }
        else
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
