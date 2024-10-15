using CharacterBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTrigger : MonoBehaviour, IOffensive
{
    Arrows arrow;
    Collider _collider;

    public void ActivateDetection()
    {
        _collider.enabled = true;
    }
    private void Awake()
    {
        arrow = GetComponentInParent<Arrows>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }
    public bool IsValidatedHit(CharacterManager manager)
    {
        return arrow.IsValidatedHit(manager);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.TryGetComponent(out HitBox hitBox))
        {
            if (arrow.IsValidatedHit(hitBox.manager))
            {
                arrow.isFly = false;
                _collider.enabled = false;
            }

            // _rigidbody.isKinematic = true;
            // transform.parent  = other.transform;
            //transform.localRotation = Quaternion.Euler(0, 0, 0);

        }


    }

   
}
