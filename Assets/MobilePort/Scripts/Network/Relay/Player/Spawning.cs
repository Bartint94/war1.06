using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting.UTP;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawning : NetworkBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] TextMeshProUGUI joincode;
        FishyUnityTransport _transport;
    // Start is called before the first frame update
    void Start()
    {
        _transport = UnityEngine.Object.FindObjectOfType<FishyUnityTransport>();
      //  joincode.text = _transport.relayManager.joinCode;
     //   FishyRelayManager.OnJoinCode += HostPlay;
        //SpawnPlayerRPC();
        
    }
    public void OnDestroy()
    {
       // FishyRelayManager.OnJoinCode -= HostPlay;
    }
    void HostPlay(string join)
    {
        _transport.StartConnection(false);
    }

  
}
