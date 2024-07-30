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
            //rigs.SetRigWightLocal(1f, RigPart.distanceAim, 0f);
            rigs.SetRigWeightServer(1f, RigPart.distanceAim, 0f);
            //characterAnimations.AnimatorAttackSpeedServer(1f);
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
        [ServerRpc]
        void ShotStart(GameObject parent)
        {
            
            
            Vector3 localPosition = aim.localPosition;
            Quaternion localRotation = aim.localRotation;


            Vector3 worldPosition = aim.TransformPoint(localPosition);
            Quaternion worldRotation = Quaternion.LookRotation(aim.forward);

            //var pool = poolzSystem.GetComponent<PoolzSystem>(); 

           // currentProjectile = poolzSystem.SpawnNob(playerData.arrows[playerData.currentArrowId].gameObject, worldPosition, worldRotation, parent, inventory);
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
            // poolzSystem.Spawn(arrow.gameObject, aim.position, aim.rotation, playerManager.transform);

          //  currentProjectile.GetComponent<Arrows>().Shot();
               arrow.ShotObserver(aim.position, aim.rotation);
             
            if (IsOwner)
            {
                //IsProjectile = true;
                Invoke(nameof(ZoomOut),zoomOutInvoke);
            }

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
        void ZoomOut()
        {
            cameraController.ToggleView(ZoomType.standard,LerpType.soft);

        }
        void ZoomIn()
        {
            cameraController.ToggleView(ZoomType.aiming,LerpType.constant);
        }
        public void EndDistance()
        {
            //characterAnimations.standardAttackId = 0;
            if(!IsOwner) { return; }
            rigs.SetRigWeightServer(0f, RigPart.distanceAim);
          //  rigs.SetRigWightLocal(0f, RigPart.distanceAim);
            characterAnimations.AttackIdServer(0);
            
            playerManager.SwitchCurrentState(playerManager.standardState);

        }

        public override void AnimationEnd()
        {

        }

        public override void BeforeSwitchState()
        {
           
        }
    }

}
