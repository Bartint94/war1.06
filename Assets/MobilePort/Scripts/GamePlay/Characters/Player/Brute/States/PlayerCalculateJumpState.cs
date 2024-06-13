using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterBehaviour
{

    public class PlayerCalculateJumpState : CharacterState
    {
        bool isMovement;


        [SerializeField] float maxJumpForce = 15f;

        [SerializeField] AnimationCurve jumpCurve;

   
        public override void InitState()
        {
            if (!IsOwner) return;

            if(playerInputs.isMobile == true)
            {
                playerInputs.isJumpStarted = false;
                playerInputs.isJumpCanceled = true;
            }
            isMovement = true;
        }

        public override void UpdateOwnerState()
        {
            characterAnimations.CalculateDirectionSpeed();
            characterAnimations.UpdateAnimatorParameters();

            playerInputs.UpdateInputs();

           // holdTime += Time.deltaTime;

            if (playerInputs.isJumpCanceled)
            {
                JumpForceValue = maxJumpForce;//jumpCurve.Evaluate(holdTime) * maxJumpForce;
                characterAnimations.JumpServer();
                isMovement = false;
                playerInputs.isJumpCanceled = false;
            }

            if (isMovement)
            {
                ReconcileStandardMovement();
            }

            if(playerInputs.isAttackStarted)
            {
                playerManager.SwitchCurrentState(playerManager.airAttackState);
            }

        }
        public void JumpForce()
        {
            if(!IsOwner) { return; }
            IsJump = true;
        }

      

        public override void UpdateServerState()
        {
            
        }

        public override void AnimationEnd()
        {
           
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
            
        }
    }
}
