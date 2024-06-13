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

        public float move;

        [SerializeField] float _moveRate = 11f;
        [SerializeField] float maxMoveVelocity = 13f;
        [SerializeField] float dashSpeed;

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
            move = .5f;
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
            /* Vector3 forces = (transform.forward * move);
             _rigidbody.AddForce(0, -9.80665f, 0);
             if (_rigidbody.velocity.magnitude < maxMoveVelocity)
             {
                 _rigidbody.AddForce(forces * _moveRate);

             }
            */
            /*    if (isDash)
                {
                    _rigidbody.AddForce(dashDir * 10f, ForceMode.Impulse);
                    isDash = false;
                }
            */


            DashForce = characterAnimations.RootMotionUpdate() * dashSpeed;
            _rigidbody.velocity = (transform.forward * DashForce) + Vector3.up * -9.8f;
            
            elapsed += Time.deltaTime;
            if(elapsed > 1f)
            {
                characterAnimations.AttackIdObservers(0);
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

      
        public override void BeforeSwitchState()
        {
            characterAnimations.AttackIdObservers(0);
            
        }
    }

}