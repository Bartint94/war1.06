using FishNet.Example;
using FishNet.Managing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineManager : MonoBehaviour
{
    NetworkManager _networkManager;
    NetworkHudCanvases networkCanvases;
    void Start()
    {
        _networkManager = FindObjectOfType<NetworkManager>();
        networkCanvases = _networkManager.GetComponentInChildren<NetworkHudCanvases>(true);
        networkCanvases.gameObject.SetActive(true);
    }
}
