using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Transform cameraTransform;
    public float shakeDuration = 0f;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }

        originalPos = cameraTransform.localPosition;
    }

  
    IEnumerator Shake()
    {
        while (shakeDuration > 0)
        {
            cameraTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;// * decreaseFactor;
            Debug.Log(shakeDuration);
            yield return new WaitForEndOfFrame();
        }
        
        cameraTransform.localPosition = originalPos;
        yield return null;
    }

    public void Shake(float duration)
    {
        shakeDuration = duration;
        StartCoroutine(Shake());    
    }
}