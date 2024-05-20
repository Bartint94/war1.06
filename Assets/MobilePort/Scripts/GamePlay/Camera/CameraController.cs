using Cinemachine;
using DG.Tweening;
using FishNet.Example.Prediction.Rigidbodies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum LerpType { constant, soft}
public enum ZoomType { standard, melee, aiming}
public class CameraController : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Transform currentFollowObject;


    public Transform aim;

    public List<CamOffset> camOffsets = new List<CamOffset>(2);

    [SerializeField] float speedMultipiler;
    Vector3 refVelocity;

    CameraShake cameraShake;


    [SerializeField] float softLerp;
    [SerializeField] float rotationSpeed;
    private void Start()
    {
        camOffsets[0].rotation = cam.localRotation;
        camOffsets[0].position = cam.localPosition;

        cameraShake = GetComponent<CameraShake>();
       
    }
    public void Shake(float duration)
    {
      //  cameraShake.Shake(duration);
    }
    public void ToggleView(ZoomType zoom ,LerpType lerp)
    {
        //LerpRot(cam,camOffsets[(int)zoom].rotation);
       if(zoom == ZoomType.aiming)
        {
       //     LerpRot(cam, aim.rotation, lerp);
        }
        else
        {
          //  LerpRot(cam, camOffsets[(int)zoom].rotation, lerp);
        }
        StopAllCoroutines();
        LerpPos(cam, camOffsets[(int)zoom].position, lerp);
        cam.transform.DOLocalRotateQuaternion(camOffsets[(int)zoom].rotation, rotationSpeed);
        

    }

    void LateUpdate()
    {
        if (currentFollowObject == null) { return; }


      //  float speed = Vector3.Distance(transform.position, currentFollowObject.position) * speedMultipiler;
        //transform.position = Vector3.MoveTowards(transform.position, currentFollowObject.position + Vector3.up, speed * Time.deltaTime);
        transform.position = currentFollowObject.transform.position;

    }
    void LerpRot(Transform current, Quaternion target)
    {
        current.DORotateQuaternion(target, .3f);
    }
    private void LerpPos(Transform current, Vector3 target, LerpType lerp)
    {
        if (lerp == LerpType.constant)
        {
            StartCoroutine(LerpPosConstant(current,target));
        }
        if(lerp == LerpType.soft)
        {
            StartCoroutine(LerpPosSoft(current,target));
        }
    }
    IEnumerator LerpPosSoft(Transform current, Vector3 target)
    {
       // float currentLerpTime = 0f;
       // Vector3 startPos = current.localPosition;
        while (Vector3.Distance(current.position, target) > 0.02f)
        {
          //  currentLerpTime += Time.deltaTime;

          
            current.localPosition = Vector3.Lerp(current.localPosition, target, Time.deltaTime * softLerp);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    IEnumerator LerpPosConstant(Transform current, Vector3 target)
    {
        float currentLerpTime = 0f;
        Vector3 startPos = current.localPosition;
        while(Vector3.Distance(current.position, target) > 0.02f) 
        {
            currentLerpTime += Time.deltaTime;

            if(currentLerpTime > .3f)
            {
                currentLerpTime = .3f;
            }

            float t = currentLerpTime / .3f;
            
            current.localPosition = Vector3.Lerp(startPos, target, t);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
[System.Serializable]
public class CamOffset
{
    public Vector3 position;
    public Quaternion rotation;
}
