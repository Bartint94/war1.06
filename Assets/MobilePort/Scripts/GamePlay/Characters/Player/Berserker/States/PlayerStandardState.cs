using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterBehaviour
{
    public class PlayerStandardState : CharacterState
    {
        public float acceleration;
        public float maxSpeed;
        [ServerRpc]
        void AttackIdServer(int id)
        {
            AttackIdObservers(id);
        }
        [ObserversRpc]
        void AttackIdObservers(int id)
        {
            characterAnimations.standardAttackId = id;

        }
        public override void InitState()
        {
            if (!IsOwner) return;

            if (!playerManager.isDistanceFighting)
                rigs.SetRigWeightServer(1f, RigPart.aim, 1f);


            characterManager.MaxSpeed = 3f;

        }

        public override void UpdateOwnerState()
        {
            if(!IsOwner) return;    
            characterManager.MaxSpeed = Mathf.Lerp(characterManager.MaxSpeed, maxSpeed, acceleration * Time.deltaTime);

            
            
            ReconcileStandardMovement();
            
       
            
            rigs.UpdateSourcePositionServer(rigs.aimSource.position, RigPart.aim);
            
            if (playerInputs.isJumpStarted)
            {
                playerManager.SwitchCurrentState(playerManager.calculateJumpState);
            }

            if(playerInputs.isAttack)
            {
                if(characterManager.isDistanceFighting)
                {
                    playerManager.SwitchCurrentState(playerManager.distanceAttackState);
                }
                else if(playerInputs.vertical>0)
                {
                    playerManager.SwitchCurrentState(playerManager.fastAttackState);//(playerManager.dashAttackState);
                }
                else if(playerInputs.vertical < 0)
                {
                    playerManager.SwitchCurrentState(playerManager.fastAttackState);//(playerManager.defAttackState);
                }
                else
                    playerManager.SwitchCurrentState(playerManager.fastAttackState);
            }

            if(playerInputs.isAttackHeavy)
            {
                playerManager.SwitchCurrentState(playerManager.attackState);
            }

       

            if(playerInputs.isBlock)
            {
                playerManager.SwitchCurrentState(playerManager.blockState);
            }
            if (playerManager.myHealth.health <= 0)
            {
                playerManager.SwitchCurrentState(playerManager.dyingState);
            }

           


        }

        public override void UpdateServerState()
        {

        }

        public override void AnimationEnd()
        {

        }

        public override void TriggerStay(Collider other)
        {
            
        }

        public override void TriggerExit(Collider other)
        {
            
        }

        public override void TriggerEneter(Collider other)
        {
          
        }

        public override void CancelState()
        {
            rigs.SetRigWeightServer(0f, RigPart.aim, 0.15f);
        }
    }
}
