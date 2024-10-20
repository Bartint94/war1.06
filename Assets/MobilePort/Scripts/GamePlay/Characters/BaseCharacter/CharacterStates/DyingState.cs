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
  
        SceneController _sceneController;
        


        
        public override void OnStartClient()
        {
            base.OnStartClient();
            
            _sceneController = FindObjectOfType<SceneController>();
        }
        
        public override void InitState()
        {
            characterManager.mainCollider.isTrigger = true;
            characterManager._rigidbody.isKinematic = true;
            characterManager.isDead = true;
            characterManager.Horizontal = 0;
            characterManager.Vertical = 0;
            characterAnimations.BoolAnimationServer(true, BoolAnimationType.die);
        }

        protected override void Update()
        {

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

        public override void UpdateOwnerState()
        {
            
        }

        public override void EndAnimation()
        {
            
        }

        public override void CancelState()
        {
            
        }
        public async Task SwitchScene()
        {
            if (_sceneController.IsLoading == false)
                await _sceneController.SwitchScene(Scenes.menu);
        }
    }

}
