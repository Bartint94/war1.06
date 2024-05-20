using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterBehaviour
{

    public class GetHitState : CharacterState
    {
        [SerializeField] Transform lerpObject;
        public Transform Weapon;
        public BodyPart bodyPartHit;

        [SerializeField] float getHitTime;
        [SerializeField] float lerp;
        [SerializeField] float trackingDistance = 5f;
        
        SkinnedMeshRenderer[] _skinnedMeshRenderer;
        

        
        [Range(0f, 1f)]
        float shaderHealth;
        float weight;


        protected override void Awake()
        {
            base.Awake();   
            
            _skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        }
      
        public override void InitState()
        {

                weight = 1f;
                shaderHealth += .01f;
                BloodShadersServer(shaderHealth);
                inventory.WeaponTriggerToggleServer(false, WeaponState.deffence);
            if(IsServer)
            {
            }
            //characterAnimations.AttackIdServer(0);
            if (IsOwner)
            {
                
               
                stopAnimationEvents = true;
               // IsGetHit = true;
               // characterAnimations.isGetHit = true;


              //  DashForce = 10f; // hard coded
               // IsDash = true;
            }
          
            
        }
           
        public override void AnimationEnd()
        {
            
        }

        public override void UpdateOwnerState()
        {

            if (IsOwner)
            {

                ReconcileStandardMovement();
                characterAnimations.CalculateDirectionSpeed();
                characterAnimations.UpdateAnimatorParameters();
                playerLook.Look();

                // var distance = Vector3.Distance(Weapon.position, transform.position);
                //if (distance < trackingDistance)

            }

                weight -= RigWeight(getHitTime);


                //  rigs.UpdateSourcePositionServer(Weapon.position, RigPart.hit);

                if (weight <= 0f)
                {
                    // rigs.SetBodyWeightServer(0f, bodyPartHit);
                    // characterAnimations.isGetHit = false;
                    rigs.SetRigWeightServer(0f, RigPart.hit, getHitTime);
                    //  rigs.SetBodyWeightServer(0f, bodyPartHit);
                    stopAnimationEvents = false;
                    characterManager.SwitchCurrentState(characterManager.standardState, "getHit");
                }
        }

        public override void UpdateServerState()
        {

        }
      

        public override void TriggerEneter(Collider other)
        {
           
        }

       

        public override void TriggerExit(Collider other)
        {

        }

        [ServerRpc(RequireOwnership = false)]
        void BloodShadersServer(float value)
        {
            BloodShadersObserver(value);
        }
        [ObserversRpc(BufferLast =true)]
        void BloodShadersObserver(float value)
        {
            foreach (var item in _skinnedMeshRenderer)
            {
                item.material.SetFloat("_Range", value + .45f);

                item.material.SetFloat("_IsBlend", 1);
            }
        }

        public override void TriggerStay(Collider other)
        {
            
        }
    }
}
