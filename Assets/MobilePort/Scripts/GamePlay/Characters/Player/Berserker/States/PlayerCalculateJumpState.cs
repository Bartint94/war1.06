using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterBehaviour
{

    public class PlayerCalculateJumpState : CharacterState
    {
        bool isMovement;
        float duration;

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
            duration = 0f;
        }

        public override void UpdateOwnerState()
        {
            characterAnimations.CalculateDirectionSpeed();
            characterAnimations.UpdateAnimatorParameters();

            playerInputs.UpdateInputs();

           // holdTime += Time.deltaTime;

            if (playerInputs.isJumpCanceled)
            {
                characterManager.JumpForceValue = maxJumpForce;//jumpCurve.Evaluate(holdTime) * maxJumpForce;
                characterAnimations.JumpServer(true);
                isMovement = false;
                playerInputs.isJumpCanceled = false;
            }
            else
            {
                duration += Time.deltaTime;
                if(duration > 1f)
                {
                    if(characterManager.IsGrounded)
                    {
                        characterAnimations.JumpServer(false);
                        playerManager.SwitchCurrentState(playerManager.standardState);
                    }
                }
            }

            if (isMovement)
            {
                ReconcileStandardMovement();
            }

            if(playerInputs.isAttack)
            {
               // playerManager.SwitchCurrentState(playerManager.airAttackState);
            }

        }
        public void JumpForce()
        {
            if(!IsOwner) { return; }
            characterManager.IsJump = true;
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

        public override void EndAnimation()
        {
            
        }

        public override void CancelState()
        {
            throw new System.NotImplementedException();
        }
    }
}
