using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterBehaviour
{

    public class CharacterStateManager : NetworkBehaviour
    {
        protected Rigidbody _rigidbody;
        public Transform body;

        public LayerMask groundMask;

        public CharacterState currentState;
        public GetHitState getHitState;
        public CharacterState standardState;
        public DodgeState dodgeState;
        public DyingState dyingState;

        public CharacterAnimationRiging rigs;
        public Health myHealth;

        
        public bool isDistanceFighting;

        protected virtual void OnDestroy()
        {

        }
        protected virtual void Awake()
        {
            rigs = GetComponent<CharacterAnimationRiging>();
            _rigidbody = GetComponent<Rigidbody>();
            myHealth = GetComponent<Health>();
        }
        public virtual void SwitchCurrentState(CharacterState state, string debug = "charscter")
        {

            if (currentState == getHitState)
            {
                if(getHitState.stopAnimationEvents == true)
                {

                    return;
                }
            }
            currentState = state;
            currentState.InitState();

        }
        private void Update()
        {
            currentState.UpdateOwnerState();
        }
        public bool CheckHitValidation(Weapon weapon)
        {

            if (weapon.manager == this)
            {
                return false;
            }
            if (weapon.currentOpponent == this)
            {
                return false;
            }
            if (weapon.currentWeaponState == WeaponState.deffence)
            {
                return false;
            }
            return true;
        }
    }



}