using CharacterBehaviour;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsRecivier : NetworkBehaviour
{
    Animator animator;
    CharacterStateManager stateManager;
    Rigidbody rb;
    float speed;
    [SerializeField] float acc;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        stateManager = GetComponent<CharacterStateManager>();
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

   
    void Update()
    {
        if(IsServer)
        {
            speed = animator.GetFloat("mSpeed");
            if(speed > 0 ) 
            Debug.Log(speed);
           
                rb.velocity = transform.forward * acc * speed;   
            

        }
    }
}
