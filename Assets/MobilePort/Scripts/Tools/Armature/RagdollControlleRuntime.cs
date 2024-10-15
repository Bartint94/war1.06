using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollControlleRuntime : NetworkBehaviour
{
    [SerializeField] Rigidbody[] ragdolRigidBody;
    [SerializeField] Collider[] ragdolColliders;
    Animator animator;
    bool isActivated;
    int bone = 4;
    float force = -200f;
    void InitComponents()
    {
       // animator = GetComponent<Animator>();
        ragdolRigidBody = GetComponentsInChildren<Rigidbody>();
        ragdolColliders = GetComponentsInChildren<Collider>();
    }
    void OnValidate()
    {
        InitComponents();
        TurnOff();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            if (isActivated )
            {
                isActivated = false;
                RagdolToggle(isActivated);
            }
            else
            {
                isActivated = true;
                RagdolToggle(isActivated);
            }
        }
        if (IsServer)
        if (Input.GetKeyDown(KeyCode.G))
        {
            AddForceO();
        }
    }
    void TurnOff()
    {
        for (int i = 1; i < ragdolRigidBody.Length; i++)
        {
            
            {
                ragdolRigidBody[i].isKinematic = true;
                ragdolColliders[i].isTrigger = true;
            }
           

        }

    }
    void RagdolToggle(bool isRagdol)
    {

        for (int i = 0; i < ragdolRigidBody.Length; i++)
        {
            if (i < 9)
            {
                ragdolRigidBody[i].isKinematic = !isRagdol;
                ragdolColliders[i].isTrigger = isRagdol;    
            }
            else
            {
                ragdolRigidBody[0].isKinematic = true;
                ragdolColliders[0].isTrigger = true;
            }
            
        }
       


        //characterAnimations.GetComponent<NetworkAnimator>().enabled = !isRagdol;
       if(animator)
        animator.enabled = !isRagdol;

        if (isRagdol)
        {
           
        }
    }
    [ServerRpc]
    void AddForce()
    {
        AddForceO();
    }
    [ObserversRpc]
    void AddForceO()
    {
        ragdolRigidBody[bone].AddForce(transform.forward* force,ForceMode.Impulse);
    }
}
