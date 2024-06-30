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

        public bool stopAnimationEvents;

        protected CharacterAnimations characterAnimations;
        protected CharacterAnimationRiging rigs;
        protected PlayerInputs playerInputs;
        protected PlayerStateManager playerManager;
        protected EnemyStateManager enemyManager;
        protected CharacterStateManager characterManager;
        protected PlayerLook playerLook;
        protected Inventory inventory;
        protected CameraController cameraController;
        protected CharacterLoad characterLoad;
        protected PlayerData playerData => characterLoad.playerData;

        protected Rigidbody _rigidbody;
        protected Animator animator;
        protected PoolzSystem poolzSystem;

        protected float holdStartTime;

        #region ReplicateData
        public bool IsJump { get; internal set; }
        public bool IsDash { get; internal set; }
        public bool IsGrounded;// { get; internal set; }
        public bool IsSprint { get; internal set; }
        public bool IsMaxSpeed { get; internal set; }
        public bool IsGetHit { get; internal set; }
        public bool IsDodge { get; internal set; }


        public float RootEulerY { get; internal set; }
        public float RootEulerX { get; internal set; }
        public float Horizontal { get; internal set; }
        public float Vertical { get; internal set; }
        public float MaxSpeed { get; internal set; }    
        public float CameraHorizontal { get; internal set; }
        public float JumpForceValue { get; internal set; }
        public float DashForce { get; internal set; }
        public bool IsStoped { get; internal set; }
        public float AimY { get; internal set; }
        public float PlayerRotX { get; internal set; }
        public float PlayerRotZ { get; internal set; }
        public bool IsProjectile { get; internal set; }
        #endregion


        #region private
        float distance;
        private float turnSmoothVel;
        #endregion


        protected virtual void Awake()
        {
            //isDistanceFighting = true;
            characterAnimations = GetComponentInChildren<CharacterAnimations>();
            playerInputs = GetComponent<PlayerInputs>();
            rigs = GetComponent<CharacterAnimationRiging>();
            characterManager = GetComponent<CharacterStateManager>();
            animator = GetComponentInChildren<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            playerLook = GetComponent<PlayerLook>();
            inventory = GetComponent<Inventory>();  
            cameraController = GetComponentInChildren<CameraController>(); 
            characterLoad = GetComponent<CharacterLoad>();
            


            if (TryGetComponent(out PlayerStateManager playerManager))
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
        private void Update()
        {
            if(IsOwner)
            {
                SurfaceAligment();
                playerInputs.UpdateInputs();
                playerLook.Look();
            }
        }

        public abstract void InitState();
        public abstract void UpdateOwnerState();
        /*{
            SurfaceAligment();
            characterAnimations.CalculateDirectionSpeed();
            characterAnimations.UpdateAnimatorParameters();
            if (IsGrounded)
            {
            }
            else
            {
                characterAnimations.OrginalAnimatorSpeed();
            }
        }*/

       
        public abstract void UpdateServerState();
        public abstract void TriggerEneter(Collider other);
        public abstract void TriggerStay(Collider other);
        public abstract void TriggerExit(Collider other);
        public abstract void AnimationEnd();
        public abstract void BeforeSwitchState();


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

                playerToGround = Quaternion.LookRotation(Vector3.ProjectOnPlane(transform.forward, info.normal), info.normal);//Quaternion.FromToRotation(Vector3.up, info.normal);//Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, info.normal), 6f);// * Time.deltaTime);//groundCurve.Evaluate(Time.deltaTime));//groundCurve.Evaluate(.25f));   
                PlayerRotX = playerToGround.x;   
                    
            }
            if (distance<1.25f)
                IsGrounded = true;
            else 
                IsGrounded = false;

        }

        protected virtual void ReconcileStandardMovement()
        {
            Horizontal = playerInputs.horizontal;
            Vertical = playerInputs.vertical;
            IsSprint = playerInputs.isSprintPerformed;
            ReconcileAimingMovement();
        }
        protected virtual void ReconcileAimingMovement()
        {
            RootEulerY = playerLook.yRot;//playerManager.RootTransform.eulerAngles.y;
          //  RootEulerX = playerManager.RootTransform.eulerAngles.x;
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
        protected virtual void StandardReturn()
        {
            characterManager.SwitchCurrentState(characterManager.standardState);
        }
        protected virtual void CancelAttack()
        {
            characterAnimations.standardAttackId = 0;

           // inventory.WeaponTriggerToggleServer(false);

        }




    }
}