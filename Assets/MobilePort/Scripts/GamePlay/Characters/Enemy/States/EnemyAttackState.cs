using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CharacterBehaviour
{

    public class EnemyAttackState : CharacterState
    {
        int attacksCount = 3;
        int currentId;

        bool isDash;

        [SerializeField] float maxDashForce = 10f;
        [SerializeField] float maxJumpForce = 10f;

        float dashForce;
        float jumpForce;    

        [SerializeField] Collider rightWeaponCollider;
        [SerializeField] Collider leftWeaponCollider;

        float elapsed;
        /*
          private void HitEnd()
          {
              if(currentId == 1)
                  rightWeaponCollider.enabled = false;
              if(currentId == 2)
                  leftWeaponCollider.enabled = false;
          }

          private void Dash()
          {
              if (currentId == 1)
                  rightWeaponCollider.enabled = true;
              if (currentId == 2)
                  leftWeaponCollider.enabled = true;
              _rigidbody.AddForce(transform.forward * dashForce, ForceMode.Impulse);
          }
          */
        public void Dash()
        {
            if (IsOwner)
            {
                isDash = true;

            }

            if (IsServer)
            {
                inventory.WeaponTriggerToggle(true, WeaponState.attack);
                // rigs.SetRigWeightObserver(1f, RigPart.aim,.3f);

            }
            /*   
               if(characterAnimations.isSwitch)
               {
                   characterAnimations.SwitchServer(false);
               }
               else
               {
                   characterAnimations.SwitchServer(true);
               }
            */
        }

        public void HitEnd()
        {
            //if (IsOwner)
            {
                isDash = false;
                //  cameraController.ToggleView(ZoomType.standard, LerpType.soft);

               // rigs.SetRigWeightServer(0f, RigPart.pelvisBoost, .2f);
                //rigs.SetRigWeightServer(0f, RigPart.aim, .2f);

                inventory.WeaponTriggerToggle(false, WeaponState.deffence);

            }

            if (IsServer)
            {
                //   inventory.WeaponTriggerToggle(false,WeaponState.deffence);

            }

        }
        public override void InitState()
        {
            elapsed = 0f;
           // enemyManager.move = 0f;
            currentId = Random.Range(1, attacksCount);
            characterAnimations.standardAttackId = currentId;
            dashForce = .5f * maxDashForce;
            enemyManager.move = .5f;
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
           

            elapsed += Time.deltaTime;
            if(elapsed > 1f)
            {
                enemyManager.SwitchCurrentState(enemyManager.standardState);
            }

            characterAnimations.UpdateAnimatorParameters();
            

          
        }

        public override void UpdateServerState()
        {
            
        }

        public override void AnimationEnd()
        {
            BeforeSwitchState();
            enemyManager.SwitchCurrentState(enemyManager.standardState);
        }

        void BeforeSwitchState()
        {
            characterAnimations.standardAttackId = 0;

        }
    }

}