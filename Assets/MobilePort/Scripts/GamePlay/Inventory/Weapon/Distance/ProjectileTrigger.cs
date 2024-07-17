using CharacterBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrigger : MonoBehaviour, IOffensive
{
    Arrows arrow;

    private void Awake()
    {
        arrow = GetComponentInParent<Arrows>();
    }
    public bool CheckTarget(CharacterStateManager manager)
    {
        return arrow.CheckTarget(manager);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.TryGetComponent(out HitBox hitBox))
        {
            if (arrow.CheckTarget(hitBox.manager))
                arrow.isFly = false;
            // _rigidbody.isKinematic = true;
            // transform.parent  = other.transform;
            //transform.localRotation = Quaternion.Euler(0, 0, 0);

        }


    }

   
}
