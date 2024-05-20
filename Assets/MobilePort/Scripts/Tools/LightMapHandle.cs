using System.Collections;
using System.Collections.Generic;
using TriInspector;
using UnityEditor;
using UnityEngine;

public class LightMapHandle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Renderer orginal;
    [SerializeField] LightDataSave saveData;
    [Button]
    void OnSave()
    {

        var renders = GetComponentsInChildren<Renderer>();
        foreach(var render in renders)
        {
            saveData._lightId.Add(render.lightmapIndex);
            saveData._lightMapScaleOffset.Add(render.lightmapScaleOffset);

        }
       
    }

 
    void Start()
    {
        var renders = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            renders[i].lightmapIndex = saveData._lightId[i];
            renders[i].lightmapScaleOffset = saveData._lightMapScaleOffset[i];
        }
    }

    [Button]
    void LoadData()
    {
        var renders = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            renders[i].lightmapIndex = saveData._lightId[i];
            renders[i].lightmapScaleOffset = saveData._lightMapScaleOffset[i];
        }
    }
}
