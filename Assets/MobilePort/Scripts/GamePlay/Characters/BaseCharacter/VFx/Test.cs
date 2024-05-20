using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : NetworkBehaviour
{
    [SerializeField] GameObject blood;
    List<GameObject> bloodlist = new List<GameObject>();
    private void OnCollisionEnter(Collision collision)
    {
     //   if (base.IsServer)
        {
            if (collision.gameObject.CompareTag("Body"))
            {
                var pos = collision.GetContact(0).point;

                bloodlist.Add(Instantiate(blood, pos, Quaternion.identity, gameObject.transform));


                Debug.Log("hit");

                OnServer();
            }
        }
    }
   

    [ServerRpc]
    void OnServer()
    {
        OnObserver();
    }
    [ObserversRpc]
    void OnObserver()
    {
        Debug.Log("hit observer");
        bloodlist[bloodlist.Count - 1].SetActive(true);
    }
}
