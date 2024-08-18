using FishNet.Component.Animating;
using FishNet.Component.Prediction;
using FishNet.Example.Prediction.Rigidbodies;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace CharacterBehaviour
{
    public class DyingState : CharacterState
    {
        [SerializeField] Rigidbody[] ragdolRigidBody;
        [SerializeField] Collider[] ragdolColliders;

        SceneController _sceneController;
        

        protected override void Awake()
        {
            base.Awake();
            ragdolRigidBody = GetComponentsInChildren<Rigidbody>();
            ragdolColliders = GetComponentsInChildren<Collider>();
            RagdolOfflineToggle(false);
        }
        public override void OnStartClient()
        {
            base.OnStartClient();
            RagdollServerToggle(false);
            _sceneController = FindObjectOfType<SceneController>();
        }
        
        public override void InitState()
        {
            RagdollServerToggle(true);
            if(enemyManager != null)
            {
              //  enemyManager.Death();
            }
            if(playerManager != null)
            {
                Invoke(nameof(SwitchScene), 5f);
            }
        }
        public async Task SwitchScene()
        {
            if (_sceneController.IsLoading == false)
                await _sceneController.SwitchScene(Scenes.menu);
        }
        public override void AnimationEnd()
        {
            
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

        public override void UpdateServerState()
        {
            
        }
        [ServerRpc(RequireOwnership = false)]
        void RagdollServerToggle(bool isRagdoll)
        {
            RagdolObserverToggle(isRagdoll);
        }
        [ObserversRpc]
        void RagdolObserverToggle(bool isRagdol)
        {
            for (int i = 1; i < ragdolRigidBody.Length; i++)
            {
                if (ragdolRigidBody[i] != null)
                    ragdolRigidBody[i].isKinematic = !isRagdol;
            }
            for (int i = 2; i < ragdolColliders.Length; i++)
            {
                if (ragdolColliders[i] != null)
                    ragdolColliders[i].isTrigger = !isRagdol;
            }
            //characterAnimations.GetComponent<NetworkAnimator>().enabled = !isRagdol;
            characterAnimations.GetComponent<Animator>().enabled = !isRagdol;
            
            if(isRagdol)
            {
                if(enemyManager != null)
                {
                   // if(enemyManager.enemyTarget  != null)
                    
                   //     Destroy(enemyManager.enemyTarget.gameObject);
                }
            }
        }
        void RagdolOfflineToggle(bool isRagdol)
        {

            for (int i = 1; i < ragdolRigidBody.Length; i++)
            {
                ragdolRigidBody[i].isKinematic = !isRagdol;
            }
            for (int i = 2; i < ragdolColliders.Length; i++)
            {
                ragdolColliders[i].isTrigger = !isRagdol;
            }
            //characterAnimations.GetComponent<NetworkAnimator>().enabled = !isRagdol;
            characterAnimations.GetComponent<Animator>().enabled = !isRagdol;

            if (isRagdol)
            {
                if (enemyManager != null)
                {
                 //  Destroy(enemyManager.enemyTarget.gameObject);
                }
            }
        }

        public override void UpdateOwnerState()
        {
            
        }

        public override void BeforeSwitchState()
        {
            
        }
    }

}
