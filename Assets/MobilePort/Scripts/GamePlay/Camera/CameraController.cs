using Cinemachine;
using DG.Tweening;
using FishNet.Example.Prediction.Rigidbodies;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum LerpType { constant, soft}
public enum ZoomType { standard, melee, aiming}
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Transform standardFollowObject;
    Transform currentFollowObject;


    public Transform aim;
    Transform standardParent;

    public List<MiscOffset> offsets = new List<MiscOffset>(2);

    [SerializeField] float speedMultipiler;
    Vector3 refVelocity;

    CameraShake cameraShake;


    [SerializeField] float softLerp;
    [SerializeField] float rotationSpeed;

    [SerializeField] float followSpeed;
    private void Start()
    {
        standardParent = cam.parent;
        cameraShake = GetComponent<CameraShake>();
        currentFollowObject = standardFollowObject;
    }
    void StandardView()
    {
        //cam.SetParent(standardParent, true);
        //cam.localRotation = offsets[0].rotation;
        //cam.localPosition = offsets[0].position;

    }
    void AimView()
    {
        
        //cam.localRotation = Quaternion.identity;
    }
    public void Shake(float duration)
    {
      //  cameraShake.Shake(duration);
    }
    public void ToggleView(ZoomType zoom ,LerpType lerp)
    {
        //LerpRot(cam,offsets[(int)zoom].rotation);
        StopAllCoroutines();
       if(zoom == ZoomType.aiming)
        {
            //     LerpRot(cam, aim.rotation, lerp);
            cam.SetParent(null, true);
            StartCoroutine(CamAimPos(cam, aim, offsets[(int)zoom].position, softLerp));
            StartCoroutine(CamAimRot(cam, aim, rotationSpeed));
            
            /* LerpPos(cam, offsets[(int)zoom].position, LerpType.constant);
            cam.localPosition = offsets[(int)zoom].position;
            cam.localRotation = offsets[(int)zoom].rotation;
            */
           
        }
        else
        {
            StandardView();
            cam.SetParent(null, true);
            StartCoroutine(CamAimPos(cam, standardParent, Vector3.zero, softLerp));
            StartCoroutine(CamAimRot(cam, standardParent, rotationSpeed));
            //  LerpRot(cam, offsets[(int)zoom].rotation, lerp);
        }
       // StopAllCoroutines();
        //LerpPos(cam, offsets[(int)zoom].position, lerp);
        //cam.transform.DOLocalRotateQuaternion(offsets[(int)zoom].rotation, rotationSpeed);

    }

    void LateUpdate()
    {
        if (currentFollowObject == null) { return; }

        

      //  float speed = Vector3.Distance(transform.position, standardFollowObject.position) * speedMultipiler;
        //transform.position = Vector3.MoveTowards(transform.position, standardFollowObject.position + Vector3.up, speed * Time.deltaTime);
        transform.position = Vector3.SmoothDamp(transform.position, currentFollowObject.transform.position,ref refVelocity, followSpeed * Time.deltaTime);

    }
   
    IEnumerator CamAimPos(Transform a, Transform b, Vector3 offset, float time)
    {
        Vector3 finalTarget = Vector3.zero;
        Vector3 startPos = a.position;
        float currentLerpTime = 0f;
        while (Vector3.Distance(a.position, finalTarget) > 0.01f)
        {
            
            finalTarget = b.position + b.right * offset.x + b.up * offset.y + b.forward * offset.z;

            currentLerpTime += Time.deltaTime;

            if (currentLerpTime > time)
            {
                currentLerpTime = time;
            }

            float t = currentLerpTime / time;

           
            a.position = Vector3.Lerp(startPos, finalTarget, t);
            yield return null;

        }
        a.SetParent(b, true);
       
        a.localPosition = Vector3.zero + offset;
        yield return null;
    }
    int debug;
    IEnumerator CamAimRot(Transform a, Transform b, float time)
    {
        
        Quaternion startRot = a.rotation;
        float currentLerpTime = 0f;
        while (Quaternion.Angle(a.rotation,b.rotation) > .01f)
        {

           // finalTarget = b.position + b.right * offset.x + b.up * offset.y + b.forward * offset.z;

            currentLerpTime += Time.deltaTime;

            if (currentLerpTime > time)
            {
                currentLerpTime = time;
            }

            float t = currentLerpTime / time;

            a.rotation = Quaternion.Lerp(startRot, b.rotation,t);
            
            yield return null;

        }
        
       // a.localRotation = Quaternion.identity;
       
        yield return null;
    }

}
[System.Serializable]
public class MiscOffset
{
    public Vector3 position;
    public Quaternion rotation;
}
