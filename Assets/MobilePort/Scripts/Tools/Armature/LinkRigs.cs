using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkRigs : MonoBehaviour
{
    [SerializeField] Transform newRigsParent;
    private void OnValidate()
    {
        var newRigs = newRigsParent.GetComponentsInChildren<Transform>();
        var originRigs = GetComponentsInChildren<Transform>();
        for (int i = 0; i < originRigs.Length; i++)
        {
            for (int j = 0; j < newRigs.Length; j++)
            {
                if (originRigs[i].name == newRigs[j].name)
                {
                    newRigs[j].SetParent(originRigs[i]);
                    newRigs[j].localPosition = Vector3.zero;
                    newRigs[j].localRotation = Quaternion.Euler(0f,0f, 0f); 
                }
            }
        }
    }
}
