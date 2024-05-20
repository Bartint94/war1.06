using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshsesConf : MonoBehaviour
{
    
    private void OnValidate()
    {
        var meshesRenderer = GetComponentsInChildren<MeshRenderer>(true);
        foreach (var item in meshesRenderer)
        {
            item.staticShadowCaster = true;
            if (item.name.Contains("D1") || item.name.Contains("D2") || item.name.Contains("D3"))
            {
                item.gameObject.SetActive(false);
            }
            else 
                item.gameObject.SetActive(true);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
