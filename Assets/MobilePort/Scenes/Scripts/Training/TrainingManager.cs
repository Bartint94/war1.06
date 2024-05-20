using CharacterBehaviour;
using FishNet.Managing.Scened;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingManager : MonoBehaviour
{
    [SerializeField] Transform trainingSpawn;
    SceneController _sceneController;
    void Start()
    {
        _sceneController = FindObjectOfType<SceneController>();
       // _sceneController.SetSpawnPoint(trainingSpawn);
        _sceneController.SinglePlayerConnection(true);
    }
    private void Update()
    {
        
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            BackMenu();
        }
    }
    async void BackMenu()
    {
        if (_sceneController == null) Debug.Log("scenecontroller not Found");
        else
        {
            if (_sceneController.IsLoading == false)
                await _sceneController.SwitchScene(Scenes.menu);
        }
    }
   
}
