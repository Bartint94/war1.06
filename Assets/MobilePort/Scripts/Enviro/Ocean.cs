using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour
{
    bool comeIn;
    public float currentValue;
    public float distance;
    [SerializeField] float speed;
    [SerializeField] Vector3 highPos;
    [SerializeField] Vector3 lowPos;
    [SerializeField] AnimationCurve curve;

    void Start()
    {
        currentValue = 0f;
    }
    private void Update()
    {
       

            
        if(comeIn)
        {
           currentValue += Time.deltaTime * speed;

            transform.position = Vector3.Lerp(lowPos, highPos, curve.Evaluate(currentValue));
            
            if(transform.position.y >= highPos.y)
            {
               // currentValue = 0f;
                comeIn = false;
            }
        }
        else
        {
            currentValue -= Time.deltaTime * speed;
            transform.position = Vector3.Lerp(lowPos,highPos,  curve.Evaluate(currentValue));
            
            if (transform.position.y <= lowPos.y)
            {
               // currentValue = 0f;
                comeIn = true;
            }
        }
    }


}
