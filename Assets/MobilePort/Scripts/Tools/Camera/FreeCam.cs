using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float speed;
    float xRot;
    float yRot;
    bool enabledCam;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (enabledCam) enabledCam = false;
            else enabledCam = true;
            
        }

        if(enabledCam)
        {
            if(Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            xRot -= Input.GetAxisRaw("Mouse Y");
            yRot += Input.GetAxisRaw("Mouse X");
            transform.rotation = Quaternion.Euler(xRot, yRot, 0f);
        }
    }
}
