using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterBehaviour
{

    public class PlayerDashAttackState : PlayerAttackState
    {
        
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
            characterAnimations.dashAttackId = id;

        }
        public override void InitState()
        {
            if (!IsOwner) { return; }
            if (playerInputs.isMobile) { playerInputs.isDodge = false; }

           
            PrepareAttack();
            // rigs.SetRigWeightServer(0f, RigPart.dodge);
        }
        void PrepareAttack()
        {
            rigs.SetRigWeightServer(0f, RigPart.aim, 0f);

            currentId = Random.Range(1, attacksCount);

            AttackIdServer(currentId);

            

            isDash = false;
        }
        protected void OnEnable()
        {

        }
        protected void OnDestroy()
        {

        }

        public override void UpdateOwnerState()
        {

            playerInputs.UpdateInputs();
            ReconcileStandardMovement();
 
            rigs.UpdateSourcePositionServer(rigs.aimSource.position, RigPart.aim);

            if (!isDash)
            {
                MaxSpeed = 0f;
            }
            else
            {

                MaxSpeed = 15f;
            
            }


            if (playerInputs.isJumpStarted)
            {
                BeforeSwitchState();
                playerManager.SwitchCurrentState(playerManager.calculateJumpState);
            }




        }

        public override void UpdateServerState()
        {

        }

        public void EndD()
        {
            BeforeSwitchState();
        }

   
        public void DashD()
        {
            if (IsOwner)
            {
                isDash = true;
                JumpForceValue = .5f * maxJumpForce;
                DashForce = .75f * maxDashForce;
                IsDash = true;
                IsJump = true;
            }
            if(IsServer)
            {
                inventory.WeaponTriggerToggle(true, WeaponState.attack);

            }

           // rigs.SetRigWeightServer(1f, RigPart.aim, 0.2f);
            // if (currentId == 1 || currentId == 5)
            //   IsDash = true;
            //if (currentId == 3 || currentId == 4)
            //  IsJump = true;
           
          
        }

        public void HitEndD()
        {
            if (IsOwner)
            {
                isDash = false; 
            }

            if(IsServer)
            {
                inventory.WeaponTriggerToggle(false, WeaponState.deffence);
            }
            

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

        public override void AnimationEnd()
        {
            //throw new System.NotImplementedException();
        }

        public override void BeforeSwitchState()
        {
            //AttackIdServer(0);
            if (IsOwner)
            {
                rigs.SetRigWeightServer(1f, RigPart.aim, .5f);
                playerManager.SwitchCurrentState(playerManager.standardState);
            }
            characterAnimations.dashAttackId = 0;
        }
    }
}
