using CharacterBehaviour;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    #region Player States

    public PlayerAttackState attackState;
    public PlayerFastAttackState fastAttackState;
    public PlayerAirAttackState airAttackState;
    public PlayerDistanceAttackState distanceAttackState;
    public PlayerCalculateJumpState calculateJumpState;
    //public PlayerDefAttackState defAttackState;
    public PlayerBlockState blockState;
    #endregion


    

    public Transform camTransform;


    public float maxSpeed;

    public float playerRotY;

    

    public override void SwitchCurrentState(CharacterState state, string p = "player")
    {
        if (!IsOwner) return;

        base.SwitchCurrentState(state, p);

    }

    protected override void Awake()
    {
        base.Awake();

        attackState = GetComponent<PlayerAttackState>();
        fastAttackState = GetComponent<PlayerFastAttackState>();
        distanceAttackState = GetComponent<PlayerDistanceAttackState>();
        calculateJumpState = GetComponent<PlayerCalculateJumpState>();
        standardState = GetComponent<PlayerStandardState>();
        blockState = GetComponent<PlayerBlockState>();

        

        currentState = standardState;
        currentState.InitState();
    }
    private void Update()
    {
        if (base.IsOwner)
        {
            currentState.UpdateOwnerState();
        }
        if (base.IsServer)
        {
            currentState.UpdateServerState();
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        currentState.TriggerEneter(other);
    }
    protected virtual void OnTriggerExit(Collider other)
    {
        currentState.TriggerExit(other);
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        currentState.TriggerStay(other);
    }

}
