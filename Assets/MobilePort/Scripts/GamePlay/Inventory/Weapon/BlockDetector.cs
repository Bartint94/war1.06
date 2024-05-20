using CharacterBehaviour;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetector : NetworkBehaviour
{
    PlayerBlockState blockState;

    public Weapon myWeapon;
    private void Awake()
    {
        blockState = GetComponentInParent<PlayerBlockState>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!IsServer) return;
        if(!myWeapon) return;
        if (other.gameObject == myWeapon.gameObject) return;

        if(other.TryGetComponent(out Weapon weapon))
        {
            SendOpponentWeapon(weapon,blockState);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!IsServer) return;
        if (!myWeapon) return;
        if (other.gameObject == myWeapon.gameObject) return;

        if (other.TryGetComponent(out Weapon weapon)) 
        {
            SendOpponentWeapon(null,blockState);
        }
    }
    [ObserversRpc(BufferLast = true)]
    void SendOpponentWeapon(Weapon weapon, PlayerBlockState blockState)
    {
        blockState.opponentWeapon = weapon;
    }
}
