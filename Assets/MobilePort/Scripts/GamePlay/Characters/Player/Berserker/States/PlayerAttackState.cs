
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using FishNet.Object;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
using GameKit.Utilities;

namespace CharacterBehaviour
{

    public class PlayerAttackState : CharacterState
    {
        public float speed;
        public float rootMotionMultipiler;

        public float acceleration;

        public float shakeDuration;
        
        protected float elapsed;


        public int attacksCount = 6;
        protected int currentId;

        [SerializeField] protected float maxJumpForce = 15f;
        [SerializeField] protected float maxDashForce = 15f;
        float duration = 1.55f;

        
        RectTransform cooldown;
        [SerializeField] RotCon rotCon;

        [SerializeField] protected AttackType attackType;
        protected override void Awake()
        {
            base.Awake();
        }
        private void Start()
        {
            cooldown = Hud.instance.AttackCooldown;
        }
        public void UpdateCooldownBar(float value)
        {
            float percentage = value / duration;

            cooldown.SetScale(new Vector3(percentage, 1f, 1f));
        }

        public override void InitState()
        {
            if(!IsOwner) { return; }
            if (playerInputs.isMobile) { playerInputs.isAttackHeavy = false; }

            PrepareAttack();
            
        }
        protected virtual void PrepareAttack()
        {
            var lastId = currentId;
            currentId = Random.Range(1, attacksCount);
            
            
            
            if(currentId == lastId)
            {
                if(currentId == 1)
                {
                    currentId++;
                }
                else if(currentId == attacksCount - 1)
                {
                    currentId--;
                }
            }

            rigs.SetRigWeightServer(0f, RigPart.aim, 0f);

            characterAnimations.AttackIdServer(currentId, attackType);

            characterManager.IsRootMotion = true;
            characterManager.RootMotionDirection = transform.forward;
            characterManager.MaxSpeed = 0f;
            elapsed = 0;
        }
    
        public override void UpdateOwnerState()
        {
            if (!IsOwner) return;

            playerManager.DashForce = playerManager.animations.RootMotionUpdate() * rootMotionMultipiler;

            elapsed += Time.deltaTime;
            UpdateCooldownBar(elapsed);
            if (elapsed > duration) 
            {
                //CancelAttackState();
            }

            playerInputs.UpdateInputs();
            ReconcileStandardMovement();



            

   
            if (playerInputs.isJumpStarted)
            {
                EndAnimation();
                playerManager.SwitchCurrentState(playerManager.calculateJumpState);
            }


            if (inventory.currentWeapon.isHBDetected == true)
            {
                cameraController.Shake(shakeDuration);
                inventory.currentWeapon.isHBDetected = false;

            }
            
        }

      

        public override void UpdateServerState()
        {
           
        }
        public override void EndAnimation()
        {
            if (IsOwner)
            {
                characterAnimations.AttackIdServer(0, attackType);
                
            }
        }
        /*

        public void IsRootMotion()
        {
            if (IsOwner)
            {
                isDash = true;

            }

            if(IsServer)
            {
                inventory.WeaponTriggerToggle(true,WeaponState.attack);


            }
        }
        */

        public void EndThis()
        {
            if(IsOwner)
            {
              //  CancelAttackState();
            }
        }
        public void WeaponDetection()
        {
            if (IsOwner)
            {
                inventory.WeaponTriggerToggleServer(false, WeaponState.deffence);

            }

            if (IsServer)
            {


            }
            
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

        public void AttackBlocked()
        {
            Debug.Log("Blooock");
            WeaponDetection();
            EndAnimation();

        }
       
        public override void AnimationEnd()
        {

        }

        public override void CancelState()
        {
            characterManager.IsRootMotion = false;
            WeaponDetection();
            EndAnimation();
            
        }
    }
}
