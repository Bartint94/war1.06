using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterBehaviour
{

    public class PlayerBlockState : CharacterState
    {
        public Weapon opponentWeapon;
        float elapsed;
        [SerializeField] float blockTime;
        [SerializeField] float blockLerpSpeed;
        [SerializeField] Transform defaultBlockPos;
        Vector3 lerpPos;
        Vector3 offset;
        
        protected override void Awake()
        {
            base.Awake();
        }
        public override void AnimationEnd()
        {

        }

        public override void InitState()
        {
            if(!IsOwner) { return; }

            offset = Vector3.up * -.2f;

            elapsed = 0f;
            
            rigs.SetRigWeightServer(1f, RigPart.blockRightArm, 0.05f);

            inventory.WeaponTriggerToggleServer(true, WeaponState.deffence);
            MaxSpeed = 2f;

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
            ReconcileStandardMovement();
            playerInputs.UpdateInputs();

            elapsed += Time.deltaTime;  
            if(elapsed >= blockTime)
            {
                EndOfTheBlock();
            }

           

                VisualBlocking();
           
        }

        void EndOfTheBlock()
        {
            inventory.WeaponTriggerToggleServer(false, WeaponState.deffence);

            rigs.SetRigWeightServer(0f, RigPart.blockRightArm, 0.1f);
            
            playerManager.SwitchCurrentState(playerManager.standardState);
        }

        public override void UpdateServerState()
        {

        }
        void VisualBlocking()
        {
            if(opponentWeapon)
            {
                lerpPos = Vector3.Lerp(lerpPos, opponentWeapon.transform.position + offset, blockLerpSpeed);
            }
            else
            {
                lerpPos = Vector3.Lerp(lerpPos, defaultBlockPos.position, blockLerpSpeed);
            }

            rigs.UpdateSourcePositionServer(lerpPos,RigPart.blockRightArm);
        }

        public override void BeforeSwitchState()
        {
      
        }
    }

}