using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ProtectionItem", menuName = "ItemCreator")]
public class ProtectionItemConfig : ScriptableObject
{
    [SerializeField] Mesh _mesh;
    public Mesh _Mesh => _mesh;
}
