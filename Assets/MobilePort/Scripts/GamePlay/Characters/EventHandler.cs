using CharacterBehaviour;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class EventHandler : NetworkBehaviour
{
    Inventory inventory;
    CharacterManager manager;
    Animator animator;
    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        manager = GetComponent<CharacterManager>();
        animator = GetComponent<Animator>();
    }
    public void Init()
    {
        if (IsServer)
        {
        }
           // animator.applyRootMotion = true;
    }
    public void End()
    {
        if (IsOwner)
        {
            
            manager.SwitchCurrentState(manager.standardState);

        }
        if(IsServer)
        {
            
        }

           // animator.applyRootMotion = false;
    }

    public void Dash()
    {
        if (IsOwner)
        {

        }

        if (IsServer)
        {
            inventory.WeaponTriggerToggle(true, WeaponState.attack);


        }
    }

    public void HitEnd()
    {
        if (IsOwner)
        {
            inventory.WeaponTriggerToggleServer(false, WeaponState.deffence);

        }

        if (IsServer)
        {


        }

    }
}
