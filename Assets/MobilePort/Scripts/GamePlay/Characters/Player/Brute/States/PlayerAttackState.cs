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
        public float dashSpeed;

        public float acceleration;

        public float shakeDuration;
        
        float elapsed;


        public int attacksCount = 6;
        int currentId;

        bool isDash;
        bool isStoped;

        [SerializeField] float maxJumpForce = 15f;
        [SerializeField] float maxDashForce = 15f;
        float duration = 1.55f;

        
        RectTransform cooldown;
        [SerializeField] RotCon rotCon;
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
            if (playerInputs.isMobile) { playerInputs.isAttackStarted = false; }

            PrepareAttack();
            
        }
        void PrepareAttack()
        {
            isStoped = false;
            isDash = true;



            var lastId = currentId;
            currentId = Random.Range(1, attacksCount);
            
                IsDash = true;
            
            
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

            characterAnimations.AttackIdServer(currentId);

            MaxSpeed = 0f;
            elapsed = 0;
        }

        public override void UpdateOwnerState()
        {
            if (!IsOwner) return;

            Debug.Log(characterAnimations.RootMotionUpdate() + " dash");
            DashForce = characterAnimations.RootMotionUpdate() * dashSpeed;

            elapsed += Time.deltaTime;
            UpdateCooldownBar(elapsed);
            if (elapsed > duration) 
            {
                CancelAttackState();
            }

            playerInputs.UpdateInputs();
            ReconcileStandardMovement();



            rigs.UpdateRotationServer(RigPart.aim, rigs.aimSource.rotation);

            /*

            if (!isDash)
            {
              //  MaxSpeed = 0f;
            }
            else if (isStoped)
            {
                MaxSpeed = 0f;
                IsStoped = true;
            }
            else
            {

                MaxSpeed = Mathf.Lerp(MaxSpeed, speed, acceleration * Time.deltaTime);
                // IsSprint = true;
                // Vertical *= 5f;
                Vertical = 1f;

            }

            */
            if (playerInputs.isJumpStarted)
            {
                BeforeSwitchState();
                playerManager.SwitchCurrentState(playerManager.calculateJumpState);
            }


            if (inventory.currentWeapon.isHBDetected == true)
            {
                // AnimatorAttackSpeedServer(0f);
                //  Invoke(nameof(CancelAttackState), 1f);
                cameraController.Shake(shakeDuration);
                inventory.currentWeapon.isHBDetected = false;
              //  elapsed = 0f;
            }
            

               // elapsed = Mathf.Lerp(elapsed, maxSpeed, Time.deltaTime * speed);
              //  characterAnimations.AnimatorAttackSpeedServer(elapsed);
        }

      
        void CancelAttackState()
        {
            IsDash = false;    
            HitEnd();
            End();
            BeforeSwitchState();
            playerManager.SwitchCurrentState(playerManager.standardState);

        }

        public override void UpdateServerState()
        {
           
        }

        public void End()
        {
            //BeforeSwitchState();
        }

        public override void BeforeSwitchState()
        {
            if (IsOwner)
            {
                characterAnimations.AttackIdServer(0);
                
            }


        }
        public void Dash()
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

        public void HitEnd()
        {
            if (IsOwner)
            {
                isDash = false;
                //  cameraController.ToggleView(ZoomType.standard, LerpType.soft);

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
        public void Stop()
        {
            isStoped = true;
        }
        public void AttackBlocked()
        {
            Debug.Log("Blooock");
            HitEnd();
            BeforeSwitchState();

        }

        public override void AnimationEnd()
        {

        }
    }
}
