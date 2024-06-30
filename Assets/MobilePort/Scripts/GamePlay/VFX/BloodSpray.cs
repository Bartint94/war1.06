using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.Formats.Alembic.Importer;
using UnityEngine.VFX;

public class BloodSpray : MonoBehaviour, ISpawnable
{
    ParticleSystem spray;
    VisualEffect vfx;
  //  [SerializeField] AlembicStreamPlayer alembicFluid;
    [SerializeField] BloodAnimation anim;
    Vector3 offset;
    Transform owner;
    float t;
    [SerializeField] float alembicSpeed;
    [SerializeField] bool hasParent;
    public void Init(Vector3 position, Quaternion rotation, GameObject owner, Inventory inventory = null)
    {
        transform.position = position;
        transform.rotation= rotation;
        if(owner)
        offset = position - owner.transform.position;
        this.owner = owner.transform;
        t = 0f;
        if(anim != null)
        {
            anim.Play();
        }
        if(spray)
        StartCoroutine(Return());
    }
    void Update()
    {
      /*  if(alembicFluid != null)
        {
            t += Time.deltaTime* alembicSpeed;
            alembicFluid.UpdateImmediately(t);
        }
      */
        if(hasParent)
            transform.position = owner.position + offset;
    }
    IEnumerator Return()
    {
        while(spray.isStopped == false)
        {

            yield return new WaitForFixedUpdate();
        }
        PoolzSystem.instance.Release(this.gameObject);
    }
}
