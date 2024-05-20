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
        [SerializeField] Weapon arrow;
        [SerializeField] Transform cam;
        [SerializeField] float maxSpeed;
       
        public override void InitState()
        {
            if (!IsOwner) { return; }
            if (playerInputs.isMobile) { playerInputs.isAttackStarted = false; }
            //rigs.SetRigWightLocal(1f, RigPart.distanceAim, 0f);
            rigs.SetRigWeightServer(1f, RigPart.distanceAim, 0f);
            cameraController.ToggleView(ZoomType.aiming,LerpType.constant);
            //characterAnimations.AnimatorAttackSpeedServer(1f);
            characterAnimations.AttackIdServer(1);
            MaxSpeed = maxSpeed;
            
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
            //ReconcileAimingMovement();
            ReconcileStandardMovement();
            playerLook.Look(true);
            characterAnimations.CalculateDirectionSpeed();
            characterAnimations.UpdateAnimatorParameters();
            
            rigs.UpdateSourcePositionServer(rigs.aimSource.position, RigPart.distanceAim);
            
           // IsStoped = true;


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

        public void ShotEnd()
        {
           // poolzSystem.Spawn(arrow.gameObject, aim.position, aim.rotation, playerManager.transform);

            if (!IsOwner) { return; }
           

            Vector3 localPosition = aim.localPosition;
            Quaternion localRotation = aim.localRotation;
            
            
            Vector3 worlsPosition = aim.TransformPoint(localPosition);
            Quaternion worldRotation = Quaternion.LookRotation(aim.forward);

            

            PoolzSystem.instance.SpawnNobServer(arrow.gameObject, worlsPosition, worldRotation,transform);
        }
/*
        [ServerRpc]
        void SpawnNobServer(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            GameObject ar = Instantiate(prefab,position,rotation);
            base.Spawn(ar);
            ar.GetComponent<ISpawnable>().Init(position, rotation, ar.transform);
            //ar.GetComponent<ISpawnable>().Init(aim.position, aim.rotation, transform);
            //GameObject ar = Instantiate(arrow.gameObject, aim.position, aim.rotation);
            //base.Spawn(ar, base.Owner);
            //SpawnProjectile();
            ActivateNobObserver(ar, position, rotation);
        }
        [ObserversRpc]
        void ActivateNobObserver(GameObject ob, Vector3 position, Quaternion rotation)
        {
            ob.SetActive(true);
            
            //GameObject ar = Instantiate(arrow.gameObject,aim.position,aim.rotation);
            // base.Spawn(ar,base.Owner);
        }
*/

        public void EndDistance()
        {
            //characterAnimations.standardAttackId = 0;
            if(!IsOwner) { return; }
            rigs.SetRigWeightServer(0f, RigPart.distanceAim);
          //  rigs.SetRigWightLocal(0f, RigPart.distanceAim);
            characterAnimations.AttackIdServer(0);
            cameraController.ToggleView(ZoomType.standard,LerpType.soft);
            
            playerManager.SwitchCurrentState(playerManager.standardState);

        }

        public override void AnimationEnd()
        {

        }
    }

}
