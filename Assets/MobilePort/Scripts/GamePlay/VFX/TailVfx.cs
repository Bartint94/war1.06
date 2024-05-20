using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailVfx : MonoBehaviour
{
    TrailRenderer trail;
    private void Awake()
    {
        trail = GetComponent<TrailRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<HitBox>() != null)
        {
            trail.enabled = true;
            Invoke(nameof(Toggle),.3f);

        }
    }
    void Toggle()
    {
        trail.enabled = false;
    }
}
