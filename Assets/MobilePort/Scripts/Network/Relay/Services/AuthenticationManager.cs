using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthenticationManager : MonoBehaviour 
{

    UiRefHolder uiRefHolder;
    [SerializeField] PlayerData playerData;
    public readonly string nameKey = "playerName";
    
    private async void Start()
    {
        uiRefHolder = UiRefHolder.instance;
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            if (AuthenticationService.Instance.IsSignedIn)
            {
                await LoadCharacter();
                uiRefHolder.loginScreen.SetActive(false);
            }
        }
    }

    public async void LoginAnonymously()
    {
        uiRefHolder.loadingScreenCanvas.SetActive(true);
        try
        {
            await Authentication.Login();
        }
        catch
        {
            Debug.Log("LogIn Error");
        }
        try
        {
            await LoadCharacter();
        }
        catch
        {
            Debug.Log("Load CLoud Error");
        }
      
            uiRefHolder.loadingScreenCanvas.SetActive(false);
       // uiRefHolder.playerName.text = playerInfo.Username;

    }
    async Task LoadCharacter()
    {
        CloudSaveData csd = FindAnyObjectByType<CloudSaveData>();

        var reasult = await csd.LoadString(nameKey);

        await uiRefHolder.customizeUi.LoadCustomization();
        await uiRefHolder.inventoryUi.LoadInventory();

        if(reasult == null)
        {
            uiRefHolder.createAccountPanel.SetActive(true);
        }
        else
        {
            playerData.playerName = reasult;
            uiRefHolder.createAccountPanel.SetActive(false);
        }
        
    }


}