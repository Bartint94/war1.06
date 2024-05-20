using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnviroLightData", menuName = "Save Enviro")]
public class LightDataSave : ScriptableObject
{
    public List<Vector4> _lightMapScaleOffset;
    public List<int> _lightId;
    public void ResetData()
    {
        _lightMapScaleOffset.Clear();
        _lightId.Clear();
    }
    
}
