using DG.Tweening;
using GameKit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodAnimation : MonoBehaviour, ISpawnable
{
    MeshRenderer[] frames;
    public int i;
    [SerializeField] float minScale;
    [SerializeField] float maxScale;
 
    [SerializeField] float duration;
    [SerializeField] float framesLength;
    [SerializeField] bool getOwner;
    [SerializeField] bool extendAnimation;
    [SerializeField] float animationFrames;
    [SerializeField] bool isRotated;
    [SerializeField] Vector3 fallOffset;
    [SerializeField] Vector3 fallScale;

    [SerializeField] AnimationCurve curvePosition;
    [SerializeField] AnimationCurve curveZPos;
    [SerializeField] AnimationCurve curveScale;
    bool isAnimated;
    float elapsedTime;

    Transform owner;
    Vector3 ownerOffset;
    

    private void Awake()
    {
        frames = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer frame in frames)
        {
            frame.enabled = false;
        }
      
        //Play();
    }
    public void Play() 
    {
        //transform.localScale = new Vector3(Random.Range(minScale, maxScale),20, 20);
        StartCoroutine(PlayAnimation());
        
    }

    IEnumerator PlayAnimation()
    {
        isAnimated = true;
        elapsedTime = 0;
        i = 0;
        while(i<framesLength)
        {
            //frames[i].enabled = false;
          
            i++;
            yield return new WaitForFixedUpdate();
            frames[i].enabled = true;
            if(i>0)
            frames[i-1].enabled = false;
        }
        
        isAnimated = false;
        
         //PoolzSystem.instance.Release(this.gameObject);
    }
    private void FixedUpdate()
    {
        if (!extendAnimation)
        {
            if (getOwner)
            {
                var offset = owner.position + ownerOffset;

                elapsedTime += Time.deltaTime;
                transform.position = (transform.forward * curveZPos.Evaluate(elapsedTime) +  offset + (new Vector3(0, curvePosition.Evaluate(elapsedTime), 0)));
           
            }
            else
            {
                elapsedTime += Time.deltaTime;
                transform.position = transform.position + (new Vector3(0, curvePosition.Evaluate(elapsedTime), 0));
            }
        }
        else
        {
            if(!isAnimated)
            {
              

                elapsedTime += Time.deltaTime;
                transform.position += transform.forward * curveZPos.Evaluate(elapsedTime) + (new Vector3(0, curvePosition.Evaluate(elapsedTime), 0));
                transform.localScale += new Vector3(curveScale.Evaluate(elapsedTime), curveScale.Evaluate(elapsedTime), curveScale.Evaluate(elapsedTime));
            }
        }
        if(elapsedTime > duration)
        {
            frames[i-1].enabled = false;
            
            PoolzSystem.instance.Release(this.gameObject);
        }

        
         
        
    }
    public void Init(Vector3 position, Quaternion rotation, GameObject owner, Inventory inventory = null)
    {
        transform.position = position;
        if(isRotated)
        transform.rotation = rotation;
        else
        {
          
        }
        this.owner = owner.transform;

        ownerOffset = position - owner.transform.position ; 

        Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 3)
        {
          //  PoolzSystem.instance.Spawn(decal,transform.position,transform.rotation);
        }
    }
}
