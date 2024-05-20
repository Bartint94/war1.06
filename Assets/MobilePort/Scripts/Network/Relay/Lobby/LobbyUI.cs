using FishNet.Managing.Scened;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;
using FishNet.Transporting;
using FishNet.Transporting.UTP;
using Unity.Services.Relay.Models;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System.Threading.Tasks;
using Unity.Services.Relay;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;

public class LobbyUI : MonoBehaviour
{
    FishyUnityTransport _transport;
    [SerializeField] GameObject MenuCamera;
    [SerializeField] GameObject lbUi;
    SceneController _sceneController;
    Lobby currentLobby;
    string _playerId;
    const string JoinCodeKey = "j";

    bool isHost;
    void Start()
    {
        _transport = UnityEngine.Object.FindObjectOfType<FishyUnityTransport>();
        _sceneController = _transport.GetComponent<SceneController>();
    }

    async Task ClientJoinAllocation()
    {
        if (!isHost)
        {
            await _sceneController.SwitchScene(Scenes.into);
        }
        _playerId = AuthenticationService.Instance.PlayerId;
        // _sceneController.ToggleInputUi(true);

        //_sceneController.SetSpawnPoint(SpawnPoints.instance.spawn[0]);
        _transport.StartConnection(false);
    }
    async Task HostCreateAllocation()
    {
        isHost = true;
        await _sceneController.SwitchScene(Scenes.into);
       // _sceneController.SetSpawnPoint(SpawnPoints.instance.spawn[0]);
        _transport.StartConnection(true);
        await ClientJoinAllocation();
    }
    public string join;
    async void HostCreateLobbyTask()
    {
        var a = await RelayService.Instance.CreateAllocationAsync(11);
        var j = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);

        var options = new CreateLobbyOptions
        {
            Data = new Dictionary<string, DataObject> { { JoinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, j) } }

        };
        currentLobby = await LobbyService.Instance.CreateLobbyAsync("NameLObyb", 16, options);
        join = j;
        lbUi.SetActive(false);

        _transport.SetRelayServerData(new RelayServerData(a, "dtls"));
        await HostCreateAllocation();
        
    }

    public void CeateOrJoinLobby()
    {
         GetRandomLobbyTask();
    }
    public async void GetRandomLobbyTask()
    {
        try
        {
            currentLobby = await Lobbies.Instance.QuickJoinLobbyAsync();

          
             var a = await RelayService.Instance.JoinAllocationAsync(currentLobby.Data[JoinCodeKey].Value);
            _transport.SetRelayServerData(new RelayServerData(a,"dtls")); //lobby.Data[JoinCodeKey].Value; //_transport.relayManager.joinCode
            await ClientJoinAllocation();
            //lbUi.SetActive(false);

        }
        catch(LobbyServiceException ex) {
            Debug.Log(ex);
            HostCreateLobbyTask();
        }
       
    }
  
    public void OnDestroy()
    {
        try
        {
            StopAllCoroutines();
            if(currentLobby != null)
            {
                if(currentLobby.HostId == _playerId)
                {
                    Lobbies.Instance.DeleteLobbyAsync(currentLobby.Id);
                }
                else
                {
                    Lobbies.Instance.RemovePlayerAsync(currentLobby.Id, _playerId);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Shutting down lobby {ex}");
        }
    }
}
