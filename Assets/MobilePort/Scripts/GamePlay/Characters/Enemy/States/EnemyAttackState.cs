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
private void WeaponDetection()
{
if(currentId == 1)
rightWeaponCollider.enabled = false;
if(currentId == 2)
leftWeaponCollider.enabled = false;
}

private void IsRootMotion()
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
           

          
                inventory.WeaponTriggerToggle(true, WeaponState.attack);
                // rigs.SetRigWeightObserver(1f, RigPart.aim,.3f);

            
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
      
                isDash = false;
                //  cameraController.ZoomOut(ZoomType.standard, LerpType.soft);

               // rigs.SetRigWeightServer(0f, RigPart.pelvisBoost, .2f);
                //rigs.SetRigWeightServer(0f, RigPart.aim, .2f);

                inventory.WeaponTriggerToggle(false, WeaponState.deffence);

            

         

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

           // DashForce = characterAnimations.RootMotionUpdate() * rootMotionMultipiler;
            _rigidbody.velocity = (transform.forward * characterManager.DashForce) + Vector3.up * -9.8f;
            
            elapsed += Time.deltaTime;
            if(elapsed > 1f)
            {
               
                characterAnimations.AttackIdObservers(0,(AttackType)Random.Range(0,2));
                enemyManager.SwitchCurrentState(enemyManager.standardState);

            }

            characterAnimations.UpdateAnimatorParameters();
            

          
        }
      
        public override void UpdateServerState()
        {
            
        }

        public override void AnimationEnd()
        {
            EndAnimation();
            enemyManager.SwitchCurrentState(enemyManager.standardState);
        }

      
        public override void EndAnimation()
        {
            characterAnimations.AttackIdObservers(0, (AttackType)Random.Range(0, 2));
            
        }

        public override void CancelState()
        {
            
        }
    }

}