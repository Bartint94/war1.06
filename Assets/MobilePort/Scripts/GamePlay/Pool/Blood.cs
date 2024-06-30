using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Blood : MonoBehaviour, ISpawnable
{
    [SerializeField] float force;
    [SerializeField] float sphere;
    [SerializeField] float minScale;
    [SerializeField] float maxScale;
    [SerializeField] float minScaleTime;
    [SerializeField] float maxScaleTime;

    public PoolzSystem poolz;
    [SerializeField] LayerMask layerMask;

    public float lifeTime;

    private void Awake()
    {
        poolz = PoolzSystem.instance;
    }
    private void Start()
    {
       // transform.position += Vector3.up * 2 + UnityEngine.Random.insideUnitSpherennnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnnop;l;l;l * 7;
    }
    /*  private void OnTriggerEnter(Collider other)
      {
          if (other.CompareTag("Character")) return;
          if (other.CompareTag("Weapon")) return;
          if (!isFly) return;  

          decal.enabled = true;

          _rigidBody.isKinematic = true;
          Destroy(_rigidBody);
          Destroy(_boxCollider);
          transform.SetParent(other.transform);
          isFly = false;
          if(other.GetComponent<HitBox>() != null)
          {
              transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.up);
          }
      }
    */
   // private void OnTriggerEnter(Collider other)
 //  {
      //  if (other.gameObject.CompareTag("Ground"))
     //   {

       // decal.enabled = true;
       // transform.SetParent(collision.transform);

       // _rigidBody.isKinematic = true;
            //Destroy(_rigidBody);
         //   _boxCollider.enabled = false;
      //  isFly = false;
        ///var normal = other.GetComponent<Collider>().normal;
        ///transform.rotation = Quaternion.LookRotation(normal);
    //    }


 //   }

    private void Update()
    {
        // if(!isFly) return;
        lifeTime += Time.deltaTime;
        if (lifeTime>= maxScaleTime)
        {
            poolz.Release(gameObject);
            lifeTime = 0;
        }
    }
 

    public void Init(Vector3 position, Quaternion rotation, GameObject owner, Inventory inventory = null)
    {
        lifeTime = 0f;

        transform.position = position;
        transform.rotation = rotation;

        force = UnityEngine.Random.Range(0, force); 
        Vector3 bloodPos = position + transform.forward * force + UnityEngine.Random.insideUnitSphere * sphere;
        Ray ray = new Ray(bloodPos, Vector3.up*-1);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,2000f,layerMask))
        {
            transform.position = hit.point;
            transform.rotation = Quaternion.LookRotation(hit.normal);
           // transform.Rotate(0f, 0f, UnityEngine.Random.Range(0, 360));
            //transform.localScale = Vector3.zero;
            transform.DOScaleX(UnityEngine.Random.Range(minScale, maxScale), UnityEngine.Random.Range(minScaleTime, maxScaleTime));
            transform.DOScaleY(UnityEngine.Random.Range(minScale, maxScale), UnityEngine.Random.Range(minScaleTime, maxScaleTime));
            transform.DOScaleZ(UnityEngine.Random.Range(minScale, maxScale), UnityEngine.Random.Range(minScaleTime, maxScaleTime));
        }
    }
   
}
