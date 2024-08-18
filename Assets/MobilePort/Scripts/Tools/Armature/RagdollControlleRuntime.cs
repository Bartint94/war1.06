using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollControlleRuntime : MonoBehaviour
{
    [SerializeField] Rigidbody[] ragdolRigidBody;
    [SerializeField] Collider[] ragdolColliders;
    Animator animator;
    bool isActivated;
  
    void InitComponents()
    {
       // animator = GetComponent<Animator>();
        ragdolRigidBody = GetComponentsInChildren<Rigidbody>();
        ragdolColliders = GetComponentsInChildren<Collider>();
    }
    void OnValidate()
    {
        InitComponents();
        RagdolToggle(false);
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
    }
    void RagdolToggle(bool isRagdol)
    {
        ragdolRigidBody[0].isKinematic = isRagdol;

        for (int i = 1; i < ragdolRigidBody.Length; i++)
        {
            ragdolRigidBody[i].isKinematic = !isRagdol;
        }

        ragdolColliders[0].isTrigger = isRagdol;
        for (int i = 1; i < ragdolColliders.Length; i++)
        {
            ragdolColliders[i].isTrigger = !isRagdol;
        }
        //characterAnimations.GetComponent<NetworkAnimator>().enabled = !isRagdol;
       if(animator)
        animator.enabled = !isRagdol;

        if (isRagdol)
        {
           
        }
    }
}
