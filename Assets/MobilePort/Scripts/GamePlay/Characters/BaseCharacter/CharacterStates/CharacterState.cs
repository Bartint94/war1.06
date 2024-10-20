using FishNet.Example.Prediction.Rigidbodies;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterBehaviour
{
    public abstract class CharacterState : NetworkBehaviour
    {

        protected float sensivity;
        protected float turnSmoothTime;

        protected CharacterAnimations characterAnimations;
        protected CharacterAnimationRiging rigs;
        protected PlayerInputs playerInputs;
        protected PlayerManager playerManager;
        protected EnemyStateManager enemyManager;
        protected CharacterManager characterManager;
        protected PlayerLook playerLook;
        protected Inventory inventory;
        protected CameraController cameraController;
        protected CharacterLoad characterLoad;
        protected PlayerData playerData => characterLoad.playerData;

        protected Rigidbody _rigidbody;
        protected Animator animator;
        protected PoolzSystem poolzSystem;

        protected float holdStartTime;

        #region private
        float distance;
        private float turnSmoothVel;
        #endregion

        protected virtual void Awake()
        {
            characterAnimations = GetComponentInChildren<CharacterAnimations>();
            playerInputs = GetComponent<PlayerInputs>();
            rigs = GetComponent<CharacterAnimationRiging>();
            characterManager = GetComponent<CharacterManager>();
            animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            playerLook = GetComponent<PlayerLook>();
            inventory = GetComponent<Inventory>();  
            cameraController = GetComponentInChildren<CameraController>(); 
            characterLoad = GetComponent<CharacterLoad>();
            


            if (TryGetComponent(out PlayerManager playerManager))
            {
                this.playerManager = playerManager;
            }
            else if (TryGetComponent(out EnemyStateManager enemyManager))
            {
                this.enemyManager = enemyManager;
            }
        }
        private void Start()
        {
            
            poolzSystem = PoolzSystem.instance;
        }
        protected virtual void Update()
        {
            SurfaceAligment();

            if(IsOwner)
            {
                playerInputs.UpdateInputs();
                playerLook.Look();
            }
        }

        public abstract void InitState();
        public abstract void UpdateOwnerState();
        public abstract void UpdateServerState();
        public abstract void TriggerEneter(Collider other);
        public abstract void TriggerStay(Collider other);
        public abstract void TriggerExit(Collider other);
        public abstract void AnimationEnd();
        public abstract void CancelState();
        public virtual void EndAnimation()
        {
            Debug.Log("Animation End Func!!!");
        }


        Ray ray;
        RaycastHit info;
        Quaternion playerToGround;
        protected virtual void SurfaceAligment()
        {
            ray = new Ray(transform.position + Vector3.up, -transform.up);
            info = new RaycastHit();
            

            if (Physics.Raycast(ray, out info, 3f, characterManager.groundMask))
            {
                distance = info.distance;

                playerToGround = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, info.normal), info.normal);
                characterManager.PlayerRotX = playerToGround.x;   
                    
            }
            if (distance<1.25f)
                characterManager.IsGrounded = true;
            else 
                characterManager.IsGrounded = false;

        }

        protected virtual void ReconcileStandardMovement()
        {
            characterManager.Horizontal = playerInputs.horizontal;
            characterManager.Vertical = playerInputs.vertical;
            characterManager.IsSprint = playerInputs.isSprintPerformed;
            ReconcileAimingMovement();
        }
        protected virtual void ReconcileAimingMovement()
        {
            characterManager.RootEulerY = playerLook.yRot;

            Aim();
        }
        float angle;
        void Aim()
        {
            angle = -playerLook.xRot * Mathf.Deg2Rad;

            var cosX = Mathf.Cos(angle);
            var sinX = Mathf.Sin(angle);

            rigs.aimSource.localPosition = new Vector3(0f, sinX, cosX);
        }

        protected virtual float RigWeight(float time)
        {
            var weight = 1f;
            var times = time / Time.deltaTime;
            var result = weight / times;
            return result;
        }
       
     




    }
}