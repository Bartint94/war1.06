using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using UnityEngine;
using Unity.Services.CloudSave.Models;


public class CloudSaveData : MonoBehaviour
{
    public async void SaveData(string saveName, int baseId)
    {
        var data = new Dictionary<string, object> { { saveName, baseId } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }
    public async void SaveString(string saveKey, string saveObject)
    {
        var data = new Dictionary<string, object> { { saveKey, saveObject } };
        await CloudSaveService.Instance.Data.Player.SaveAsync(data);
    }
    public async Task<string> LoadString(string key)
    {
        try
        {
            Dictionary<string, Unity.Services.CloudSave.Models.Item> serverData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });
            var load = serverData[key].Value;
            var reasult = load.GetAsString();
            return reasult;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return null;
        }
    }
    public async Task<int> LoadData(string key)
    {
        try
        {
            Dictionary<string, Unity.Services.CloudSave.Models.Item> serverData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { key });
            var load = serverData[key].Value;
            int reasult = Int32.Parse(load.GetAsString());
            return reasult;
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            return 0;
        }
    }
}

