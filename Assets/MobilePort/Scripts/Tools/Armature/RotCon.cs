using CharacterBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotCon : MonoBehaviour
{
    [SerializeField] Transform rot;
    [SerializeField] bool isBowAiming = true;//=> manager.isDistanceFighting;
    [SerializeField] float offsetx;
    [SerializeField] float offsety;
    [SerializeField] float offsetz;
    CharacterManager manager;
    public Quaternion test;
    private void Awake()
    {
        manager = GetComponentInParent<CharacterManager>();
        isBowAiming = true;
    }
    void Update()
    {
        if(isBowAiming)
        {
            return;
        }
        else
        transform.rotation = rot.rotation;
    }
}
