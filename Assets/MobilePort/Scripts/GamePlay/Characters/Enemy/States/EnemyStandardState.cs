using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterBehaviour
{

    public class EnemyStandardState : CharacterState
    {
        Transform targetTransform;
        EnemyTarget enemyTarget;
        public float maxMoveVelocity;
        public float _moveRate;
        public float attackDistance;

        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            enemyTarget = enemyManager.enemyTarget; 
            targetTransform = enemyTarget.transform; 
        }
        public override async void InitState()
        {
           // await enemyTarget.LookForCloserTarget();
        }
      
        void Die()
        {
            enemyManager.SwitchCurrentState(enemyManager.dyingState);
        }

        public override void UpdateOwnerState()
        {
            if (IsGrounded)
            {
                characterAnimations.CalculateDirectionSpeed();
                characterAnimations.UpdateAnimatorParameters();
            }

           
          

            transform.rotation = Quaternion.LookRotation(new Vector3(targetTransform.position.x, transform.position.y ,targetTransform.position.z) - transform.position);
            
            _rigidbody.AddForce(0, -9.80665f, 0);
            if (_rigidbody.velocity.magnitude < maxMoveVelocity)
            {
                _rigidbody.AddForce(transform.forward * _moveRate);

            }


            if (enemyTarget.ownerDistance < attackDistance && enemyTarget.isTargetSpotted)
            {
                enemyManager.SwitchCurrentState(enemyManager.attackState);

            }
            
            
        }

        public override void UpdateServerState()
        {

        }
        public override void AnimationEnd()
        {

        }
       
        public override void TriggerEneter(Collider other)
        {
            if (other.CompareTag("Weapon"))
            {
         // characterManager.SwitchCurrentState(characterManager.dodgeState);
            }
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
