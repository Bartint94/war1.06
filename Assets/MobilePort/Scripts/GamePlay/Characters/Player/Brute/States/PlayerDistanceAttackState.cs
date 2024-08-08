using FishNet.Connection;
using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using UnityEngine;

namespace CharacterBehaviour
{
    public class PlayerDistanceAttackState : CharacterState
    {
        public Transform aim;
        [SerializeField] Transform cam;
        [SerializeField] float maxSpeed;
        public GameObject currentProjectile;
        public Arrows arrow;
        [SerializeField] float zoomInInvoke;
        [SerializeField] float zoomOutInvoke;
        public override void InitState()
        {
            if (!IsOwner) { return; }
            if (playerInputs.isMobile) { playerInputs.isAttackStarted = false; }

            rigs.SetRigWeightServer(1f, RigPart.distanceAim, 0f);

            characterAnimations.AttackIdServer(1);
            MaxSpeed = maxSpeed;
            ShotStart(gameObject);
            Invoke(nameof(ZoomIn), zoomInInvoke);  
        }
        protected void OnEnable()
        {
        }

        protected void OnDestroy()
        {
        }
        public override void UpdateOwnerState()
        {

            if (!IsOwner) return;
            playerInputs.UpdateInputs();

            ReconcileStandardMovement();
            playerLook.Look(true);
            characterAnimations.CalculateDirectionSpeed();
            characterAnimations.UpdateAnimatorParameters();
            
            rigs.UpdateSourcePositionServer(rigs.aimSource.position, RigPart.distanceAim);


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
        [ServerRpc]
        void ShotStart(GameObject parent)
        {
            
            
            Vector3 localPosition = aim.localPosition;
            Quaternion localRotation = aim.localRotation;


            Vector3 worldPosition = aim.TransformPoint(localPosition);
            Quaternion worldRotation = Quaternion.LookRotation(aim.forward);

            ArrowRef(worldPosition,worldRotation);
        }
        [ObserversRpc] 
        void ArrowRef(Vector3 pos, Quaternion rot)
        {
            currentProjectile = poolzSystem.SpawnNob(playerData.arrows[playerData.currentArrowId].gameObject, pos, rot, aim.gameObject, inventory);
            
            arrow = currentProjectile.GetComponent<Arrows>();
        }
        public void ShotEnd()
        {

               arrow.ShotObserver(aim.position, aim.rotation);
             
            if (IsOwner)
            {
                Invoke(nameof(ZoomOut),zoomOutInvoke);
            }

        }

        void ZoomOut()
        {
            if(cameraController != null) 
            
                cameraController.ToggleView(ZoomType.standard,LerpType.soft);

        }
        void ZoomIn()
        {
            if(cameraController != null)

                cameraController.ToggleView(ZoomType.aiming,LerpType.constant);
        }
        public void EndDistance()
        {

            if(!IsOwner) { return; }
            rigs.SetRigWeightServer(0f, RigPart.distanceAim);

            characterAnimations.AttackIdServer(0);
            
            playerManager.SwitchCurrentState(playerManager.standardState);

        }

        public override void AnimationEnd()
        {

        }

        public override void BeforeSwitchState()
        {
            if (IsOwner)
            {
                characterAnimations.AttackIdServer(0);
                ZoomOut();
                Debug.Log("bef");
            }
        }
    }

}
