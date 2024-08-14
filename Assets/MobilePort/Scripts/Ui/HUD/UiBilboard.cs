using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBilboard : MonoBehaviour
{
    Camera cam;
    void Start()
    {
        
    }

   
    void LateUpdate()
    {
        if (Camera.main)
        {
            transform.LookAt(Camera.main.transform.position);
            
        }
    }
}
