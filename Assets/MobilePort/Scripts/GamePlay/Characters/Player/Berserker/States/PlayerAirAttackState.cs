using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterBehaviour
{

    public class PlayerAirAttackState : CharacterState

    {
        int attacksCount = 6;
        int currentId;

        bool isDash;
        [SerializeField] float maxJumpForce = 15f;
        [SerializeField] float maxDashForce = 15f;
        protected void OnEnable()
        {
           
        }
        protected void OnDestroy()
        {
           
        }
        public override void InitState()
        {
            if (!IsOwner) { return; }
            if (playerInputs.isMobile) { playerInputs.isAttack = false; }

            rigs.SetRigWeightServer(0f, RigPart.dodge);

            currentId = Random.Range(1, attacksCount);
            characterAnimations.standardAttackId = currentId;



            if (currentId == 3 || currentId == 4 || currentId == 1)
            {
                characterManager.JumpForceValue = .5f * maxJumpForce;
                characterManager.IsJump = true;
                Debug.Log("3");
            }
            if (currentId == 1 || currentId == 5)
            {
                characterManager.DashForce = .75f * maxDashForce;
                Debug.Log("1");
            }


            isDash = false;
        }

        public override void AnimationEnd()
        {
            
        }

        public override void TriggerEneter(Collider other)
        {
            
        }

        public override void TriggerExit(Collider other)
        {
            
        }

        public override void TriggerStay(Collider other)
        {
           
        }
        public override void UpdateOwnerState()
        {
           
            if (!isDash)
            {
                //IsStoped = true;
            }
            if (characterManager.IsGrounded)
            {
                characterAnimations.isJump = false;
                characterAnimations.standardAttackId = 0;


                playerManager.SwitchCurrentState(playerManager.standardState);
            }
        }

        public override void UpdateServerState()
        {
            

          
        }
      

        public override void EndAnimation()
        {
          
        }

        public override void CancelState()
        {
           
        }
    }
}
