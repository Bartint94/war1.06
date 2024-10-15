using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace CharacterBehaviour
{

    public class PlayerFastAttackState : PlayerAttackState
    {
        public override void InitState()
        {
            if (!IsOwner) { return; }
            if (playerInputs.isMobile) { playerInputs.isAttack = false; }

            PrepareAttack();

        }
        protected override void PrepareAttack()
        {
            attackType = AttackType.standard;

            var lastId = currentId;
            currentId = Random.Range(1, attacksCount);

            rigs.SetRigWeightServer(1f, RigPart.aim, 1f);

            characterAnimations.AttackIdServer(currentId);
           
            elapsed = 0f;
            
        }
        
        public override void UpdateOwnerState()
        {
            if (!IsOwner) return;

            rigs.UpdateSourcePositionServer(rigs.aimSource.position, RigPart.aim);

            playerInputs.UpdateInputs();
            ReconcileStandardMovement();

            elapsed += Time.deltaTime;
            if (elapsed > .5f)
            {
               
            }
            


        }

        public override void CancelState()
        {
            rigs.SetRigWeightServer(0f, RigPart.aim, .15f);
            characterManager.IsRootMotion = false;
            WeaponDetection();
            EndAnimation();

        }


    }
}