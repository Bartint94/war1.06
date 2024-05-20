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
            
            //rigs.SetRigWightLocal(0f,RigPart.)
            //rigs.SetRigWeightServer(0f, RigPart.hit);
            Debug.Log("standard state");
            MaxSpeed = 3f;
            //AttackIdServer(0);
        }

        public override void UpdateOwnerState()
        {
            MaxSpeed = Mathf.Lerp(MaxSpeed, maxSpeed, acceleration * Time.deltaTime);

            
            
            ReconcileStandardMovement();
            
            playerLook.Look();
            
            rigs.UpdateSourcePositionServer(rigs.aimSource.position, RigPart.aim);
            
            if (playerInputs.isJumpStarted)
            {
                playerManager.SwitchCurrentState(playerManager.calculateJumpState);
            }

            if(playerInputs.isAttackStarted)
            {
                if(characterManager.isDistanceFighting)
                {
                    playerManager.SwitchCurrentState(playerManager.distanceAttackState);
                }
                else if(playerInputs.vertical>0)
                {
                    playerManager.SwitchCurrentState(playerManager.attackState);//(playerManager.dashAttackState);
                }
                else if(playerInputs.vertical < 0)
                {
                    playerManager.SwitchCurrentState(playerManager.attackState);//(playerManager.defAttackState);
                }
                else
                    playerManager.SwitchCurrentState(playerManager.attackState);
            }

            if(playerInputs.isDodge)
            {
                playerManager.SwitchCurrentState(playerManager.dashAttackState);
            }

            if(playerInputs.isBlock)
            {
                playerManager.SwitchCurrentState(playerManager.blockState);
            }


            if(Input.GetKeyDown(KeyCode.K)) 
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
    }
}
