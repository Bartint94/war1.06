using FishNet.Managing.Scened;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    SceneController _sceneController;
    CanvasScaler canvas;
    [SerializeField] float time = 0f;
    void Start()
    {
        canvas = GetComponentInChildren<CanvasScaler>();
        Cursor.lockState = CursorLockMode.Confined;
        _sceneController = FindAnyObjectByType<SceneController>();
        canvas.scaleFactor = 0.01f;
        StartCoroutine(ScaleUpMenu());
    }
    private IEnumerator ScaleUpMenu()
    {
        
        while(canvas.scaleFactor < 1f)
        {
            time += 0.0003f;
            canvas.scaleFactor = Mathf.SmoothStep(canvas.scaleFactor, 1f, time);

            yield return new WaitForEndOfFrame();
        }
    }

    public async void StoryModeButton()
    {
        if (_sceneController == null) Debug.Log("scenecontroller not Found");
        else
        {
            if(_sceneController.IsLoading == false)
            await _sceneController.SwitchScene(Scenes.into);
        }
    }
    public async void TriningButton()
    {
        if (_sceneController == null) Debug.Log("scenecontroller not Found");
        else
        {
            if (_sceneController.IsLoading == false)
            await _sceneController.SwitchScene(Scenes.training);
        }

    }
    public async void OnlineButton()
    {
        if (_sceneController == null) Debug.Log("scenecontroller not Found");
        else
        {
            if (_sceneController.IsLoading == false)
            await _sceneController.SwitchScene(Scenes.online);
        }
    }
}
