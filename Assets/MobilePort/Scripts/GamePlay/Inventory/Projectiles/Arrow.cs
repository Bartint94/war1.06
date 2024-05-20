using GameKit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Item
{
    Rigidbody _rigidbody;
    ParticleSystem fireVfx;
    public bool isIntro = false;
     void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        fireVfx = GetComponentInChildren<ParticleSystem>(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isIntro) { return; }
        SwitchKinematic(true);
        StartCoroutine(ScaleFire());
    }
    public void FireSwicth(bool isPlay)
    {
        fireVfx.gameObject.SetActive(isPlay);
        if (isPlay)
        {
            fireVfx.Play();
        }
        else
            fireVfx.Stop();
    }
    IEnumerator ScaleFire()
    {
        float maxScale = Random.Range(.2f, 3f);

        while (fireVfx.transform.localScale.x < maxScale)
        {

            fireVfx.transform.SetScale(fireVfx.transform.GetScale() + fireVfx.transform.GetScale() * .02f);

            yield return new WaitForSeconds(.2f);
        }
    }
    public void SwitchKinematic(bool isKinematic)
    {
        if(isKinematic)
        {
            _rigidbody.isKinematic = true;
        }
        else
        {
            _rigidbody.isKinematic = false;
        }
    }
  
}
