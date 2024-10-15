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
        public Vector3 hitPosition;
        public List<BodyPart> bodyPartHit;

        [SerializeField] float getHitTime;
        [SerializeField] float lerp;
        
        
        SkinnedMeshRenderer[] _skinnedMeshRenderer;
        

        
        [Range(0f, 1f)]
        float shaderHealth;
        float weight;
        [SerializeField] float rootMotionMultipiler;
        internal Vector3 direction;

        protected override void Awake()
        {
            base.Awake();   
            
            _skinnedMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        }
      
        public override void InitState()
        {
          
            if (IsOwner)
            {
                characterManager.RootMotionDirection = direction;
                Debug.Log($"direction  = {direction}");
                characterManager.IsRootMotion = true;
                characterAnimations.BoolAnimationServer(true, BoolAnimationType.getHit);


                weight = 1f;
                shaderHealth += .01f;
                BloodShadersServer(shaderHealth);



                rigs.UpdateSourcePositionServer(hitPosition,RigPart.hit);
                rigs.SetRigWeightServer(1f, RigPart.hit, lerp);

                foreach (var part in bodyPartHit)
                {
                    rigs.SetBodyWeightServer(1f, part, 0f);
                }

     
            }
        }

        public override void UpdateOwnerState()
        {

            if (IsOwner)
            {
                playerManager.DashForce = playerManager.animations.RootMotionUpdate() * rootMotionMultipiler;
                characterAnimations.CalculateDirectionSpeed();
                characterAnimations.UpdateAnimatorParameters();
                playerLook.Look();

                weight -= RigWeight(getHitTime);



                if (weight <= 0f)
                {
                   // characterManager.SwitchCurrentState(characterManager.standardState, "getHit");

                }

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

        public override void CancelState()
        {
            
            rigs.SetRigWeightServer(0f, RigPart.hit, lerp);
               

            characterManager.IsRootMotion = false;
            characterAnimations.BoolAnimationServer(false, BoolAnimationType.getHit);
            
            
        }

        public override void AnimationEnd()
        {
          
        }
    }
}
