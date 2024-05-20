using FishNet.Transporting.UTP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public static Hud instance;

    public PlayerInputs playerInputs;

    SceneController sceneController;
    FishyUnityTransport transport;



    [SerializeField] MobileInputAccess _mobileInputAccess;
    public MobileInputAccess MobileInputAccess => _mobileInputAccess;

    [SerializeField] RectTransform _attackCooldown;
    public RectTransform AttackCooldown => _attackCooldown;
    
    
    [SerializeField] Toggle isSimpleSteering;
    [SerializeField] Toggle isMobile;
    private void Awake()
    {
      instance = this;
    }
    public void Init(PlayerInputs playerInputs)
    {

        this.playerInputs = playerInputs;

        isSimpleSteering.isOn = playerInputs.isSimpleSteering;
        isMobile.isOn = playerInputs.isMobile;
    }
    public async void Quite()
    {
        sceneController = FindAnyObjectByType<SceneController>();
        transport = sceneController.GetComponent<FishyUnityTransport>();
        transport.StopConnection(false);
        await sceneController.SwitchScene(Scenes.menu);
    }
    public void SwitchSteering() 
    {
        if(playerInputs == null)
        {
            Debug.Log("playerInput = null");
            return;
        }
        playerInputs.isSimpleSteering = isSimpleSteering.isOn;
    }
    public void SwitchPlatform()
    {
        if (playerInputs == null)
        {
            Debug.Log("playerInput = null");
            return;
        }
        playerInputs.isMobile = isMobile.isOn;
    }
    private void Update()
    {
        Debug.Log(playerInputs);
    }
}
