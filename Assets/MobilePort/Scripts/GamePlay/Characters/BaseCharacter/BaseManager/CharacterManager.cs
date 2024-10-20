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

    public class CharacterManager : NetworkBehaviour
    {
        public Rigidbody _rigidbody;
        public Transform body;
        public Inventory inventory;

        public LayerMask groundMask;

        public CharacterState lastState;
        public CharacterState currentState;
        public GetHitState getHitState;
        public CharacterState standardState;
        public DodgeState dodgeState;
        public DyingState dyingState;

        public CharacterAnimations animations;
        public CharacterAnimationRiging rigs;
        public Health myHealth;

        public float moveRate = 15f;

        public bool isDistanceFighting;

        public bool isBot;



        #region ReplicateData
        public bool IsJump { get; internal set; }
        public bool IsRootMotion { get; internal set; }
        public bool IsGrounded;// { get; internal set; }
        public bool IsSprint { get; internal set; }
        public bool IsMaxSpeed { get; internal set; }
        public bool IsGetHit { get; internal set; }
        public bool IsDodge { get; internal set; }


        public float RootEulerY { get; internal set; }

        public float Horizontal { get; internal set; }
        public float Vertical { get; internal set; }
        public float MaxSpeed { get; internal set; }
        public float CameraHorizontal { get; internal set; }
        public float JumpForceValue { get; internal set; }
        public float DashForce { get; internal set; }
        public Vector3 RootMotionDirection { get; internal set; }
        public bool IsStoped { get; internal set; }
        public float AimY { get; internal set; }
        public float PlayerRotX { get; internal set; }
        public float PlayerRotZ { get; internal set; }
        public bool IsProjectile { get; internal set; }
        #endregion


      


        protected virtual void OnDestroy()
        {

        }
        protected virtual void Awake()
        {
            rigs = GetComponent<CharacterAnimationRiging>();
            _rigidbody = GetComponent<Rigidbody>();
            myHealth = GetComponent<Health>();
            getHitState = GetComponent<GetHitState>();
            animations = GetComponent<CharacterAnimations>();
        }
        public virtual void SwitchCurrentState(CharacterState state, string debug = "charscter")
        {
            
            currentState.CancelState();

            currentState = state;
            currentState.InitState();

        }
        private void Update()
        {
            currentState.UpdateOwnerState();
        }
        public bool CheckHitValidation(IOffensive weapon)
        {

            return weapon.IsValidatedHit(this);
           
          
        }
    }



}