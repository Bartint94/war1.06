using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.LowLevel;

namespace CharacterBehaviour
{

    public class PlayerDefAttackState : CharacterState
    {
        int attacksCount = 4;
        int currentId;

        bool isDash;


        [SerializeField] float maxJumpForce = 15f;
        [SerializeField] float maxDashForce = 15f;

        [ServerRpc]
        void AttackIdServer(int id)
        {
            AttackIdObservers(id);
        }
        [ObserversRpc]
        void AttackIdObservers(int id)
        {
            characterAnimations.defAttackId = id;

        }
        public override void InitState()
        {
            if (!IsOwner) { return; }
            if (playerInputs.isMobile) { playerInputs.isAttackStarted = false; }

            PrepareAttack();
            // rigs.SetRigWeightServer(0f, RigPart.dodge);

        }
        void PrepareAttack()
        {
            rigs.SetRigWeightServer(0f, RigPart.aim, 0f);

            currentId = Random.Range(1, attacksCount);

            AttackIdServer(currentId);

          /*  if (currentId == 3 || currentId == 4 || currentId == 1)
            {
                JumpForceValue = .5f * maxJumpForce;
                IsJump = true;
                Debug.Log("3");
            }
            if (currentId == 1 || currentId == 5)
            {
                DashForce = .75f * maxDashForce;
                Debug.Log("1");
            }

            isDash = false;
            */
        }
        protected void OnEnable()
        {

        }
        protected void OnDestroy()
        {

        }

        public override void UpdateOwnerState()
        {

            // SurfaceAligment();
            //  characterAnimations.CalculateDirectionSpeed();
            //characterAnimations.UpdateAnimatorParameters();
            //   characterAnimations.OrginalAnimatorSpeed(); // jesli probblemy z polaczeniem


            //playerInputs.UpdateInputs();
            // ReconcileStandardMovement();
            ReconcileAimingMovement();
            playerLook.Look();
            Vertical = playerInputs.vertical*.3f;
            // characterAnimations.CalculateDirectionSpeed();
            //characterAnimations.UpdateAnimatorParameters();


           // rigs.UpdateSourcePositionServer(rigs.aimSource.position, RigPart.aim);



            if (playerInputs.isJumpStarted)
            {
                BeforeSwitchState();
                playerManager.SwitchCurrentState(playerManager.calculateJumpState);
            }




        }

        public override void UpdateServerState()
        {

        }

        public override void AnimationEnd()
        {
            
            BeforeSwitchState();
           // playerManager.SwitchCurrentState(playerManager.standardState);
        }

        void OnDash()
        {
            if (!IsOwner) return;

           // rigs.SetRigWeightServer(1f, RigPart.aim, 0.2f);
           // isDash = true;
            inventory.WeaponTriggerToggleServer(true, WeaponState.attack);
           // if (currentId == 1 || currentId == 5)
             //   IsDash = true;
         //   if (currentId == 3 || currentId == 4)
            //    IsJump = true;
          //  IsDash = true;

        }

        void OnHitEnd()
        {
            if (!IsOwner) return;
            inventory.WeaponTriggerToggleServer(false, WeaponState.deffence);


        }

        public override void TriggerEneter(Collider other)
        {

        }

        public override void TriggerStay(Collider other)
        {

        }

        public override void TriggerExit(Collider other)
        {

        }

        public override void BeforeSwitchState()
        {
            AttackIdServer(0);
        }
    }

}