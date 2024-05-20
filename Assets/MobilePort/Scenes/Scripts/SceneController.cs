using FishNet.Component.Spawning;
using FishNet.Managing;
using FishNet.Transporting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scenes {menu, into, training, online}
public class SceneController : MonoBehaviour
{
    private LocalConnectionState _clientState = LocalConnectionState.Stopped;
    private LocalConnectionState _serverState = LocalConnectionState.Stopped;

    NetworkManager _networkManager;
    PlayerSpawner _spawner;

   // [SerializeField] Slider _slider;
    [SerializeField] GameObject loadScreen;
    [SerializeField] TextMeshProUGUI dmg;
    [SerializeField] GameObject inputUi;

    bool isLoading;
    public bool IsLoading => isLoading;

    public void DmgText(string text)
    {
      //  dmg.text = text;
    }
    private void Awake()
    {

        //_slider.gameObject.SetActive(false);
        _networkManager = GetComponent<NetworkManager>();
        _spawner = GetComponent<PlayerSpawner>();
    }
    private void Start()
    {
        if (_networkManager == null)
        {
            Debug.LogError("NetworkManager not found, HUD will not function.");
            return;
        }
        else
        {
            // _networkManager.ServerManager.OnServerConnectionState += ServerManager_OnServerConnectionState;
            // _networkManager.ClientManager.OnClientConnectionState += ClientManager_OnClientConnectionState;
        }
    }
    private void OnDestroy()
    {
        if (_networkManager == null)
            return;

        //  _networkManager.ServerManager.OnServerConnectionState -= ServerManager_OnServerConnectionState;
        // _networkManager.ClientManager.OnClientConnectionState -= ClientManager_OnClientConnectionState;
    }

    private void ClientManager_OnClientConnectionState(ClientConnectionStateArgs args)
    {
        _clientState = args.ConnectionState;
    }

    private void ServerManager_OnServerConnectionState(ServerConnectionStateArgs args)
    {
        _serverState = args.ConnectionState;
    }
    public void SinglePlayerConnection(bool connect)
    {
        StartHostConnection(connect);
        StartClientConnection(connect);
    }
    void StartHostConnection(bool isConnect)
    {
        if (_networkManager == null)
            return;

        if (isConnect)
        {

            _networkManager.ServerManager.StartConnection();

        }
        else
        {

            _networkManager.ServerManager.StopConnection(true);
        }


    }
    void StartClientConnection(bool isConnect)
    {
        if (_networkManager == null)
            return;
        if (isConnect)
        {

            _networkManager.ClientManager.StartConnection();
        }
        else
        {
            _networkManager.ClientManager.StopConnection();
        }
    }
    public async Task SwitchScene(Scenes scene)
    {
        isLoading = true;

        loadScreen.gameObject.SetActive(true);
         var task = SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Single);

      //  task.allowSceneActivation = false;


        do
        {
            await Task.Delay(100);
            //_slider.value = task.progress;
        } while (task.progress < .9f);

         //task.allowSceneActivation = true;
         await Task.Delay(2000);

        loadScreen.gameObject.SetActive(false);
        //_slider.value = 0f;
        //    SinglePlayerConnection(false);
        isLoading = false;

    }


    public void SetSpawnPoint(Transform spawnPos)
    {
        _spawner.Spawns[0] = spawnPos;
        
    }
    public void ToggleInputUi(bool toggle)
    {
        inputUi.SetActive(toggle);
    }
}

