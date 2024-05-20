using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableInstantiator : MonoBehaviour
{
    [SerializeField] AssetReferenceGameObject forest;

    public void Load()
    {
        forest.LoadAssetAsync().Completed += OnForestLoaded;
    }

    private void OnForestLoaded(AsyncOperationHandle<GameObject> handle)
    {
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(handle.Result);
           // handle.Result.GetComponent<LightMapHandle>().OnLoad();

        }
        else
        {
            Debug.LogError("Load Forest Failed!");
        }
    }
}
