using FishNet;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using UnityEngine;

namespace CharacterBehaviour
{
    public class PlayerStateManager : CharacterStateManager
    {
#if !PREDICTION_V2
        #region Types.
        public struct ReconcileData : IReconcileData
        {
            public Vector3 Position;
            public Quaternion Rotation;
            public Vector3 Velocity;
            public Vector3 AngularVelocity;
            public ReconcileData(Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity)
            {
                Position = position;
                Rotation = rotation;
                Velocity = velocity;
                AngularVelocity = angularVelocity;
                _tick = 0;
            }

            private uint _tick;
            public void Dispose() { }
            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
        }
        #endregion


        public enum CameraStyle {Fps, Rpg}
        public CameraStyle currentCameraStyle = CameraStyle.Fps;
        

        #region Player States
        public PlayerAttackState attackState;
        public PlayerAirAttackState airAttackState;
        public PlayerDistanceAttackState distanceAttackState;
        public PlayerCalculateJumpState calculateJumpState;
        public PlayerDashAttackState dashAttackState;
       //public PlayerDefAttackState defAttackState;
        public PlayerBlockState blockState;
        #endregion

        public Transform RootTransform;

        Vector3 dashDir;
        float maxSpeed;


        Animator anim;
        public override void SwitchCurrentState(CharacterState state, string p = "player")
        {
            if (!IsOwner) return;
           
            base.SwitchCurrentState(state,p);

        }

        protected override void Awake()
        {
            base.Awake();
            anim = GetComponent<Animator>();   
            currentState = standardState;
            currentState.InitState();
            InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;
            InstanceFinder.TimeManager.OnPostTick += TimeManager_OnPostTick;
        }
        protected void Start()
        {

            Cursor.lockState = CursorLockMode.Locked;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (InstanceFinder.TimeManager != null)
            {
                InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
                InstanceFinder.TimeManager.OnPostTick -= TimeManager_OnPostTick;
            }
        }

        public override void OnStartClient()
        {
            base.PredictionManager.OnPreReplicateReplay += PredictionManager_OnPreReplicateReplay;
            RootTransform.gameObject.SetActive(IsOwner);
            attackState.enabled = IsOwner;
            RootTransform.SetParent(null, true);
        }
        public override void OnStopClient()
        {
            base.PredictionManager.OnPreReplicateReplay -= PredictionManager_OnPreReplicateReplay;
        }
       
        protected virtual void OnTriggerEnter(Collider other)
        {
            currentState.TriggerEneter(other);
        }
        protected virtual void OnTriggerExit(Collider other)
        {
            currentState.TriggerExit(other);
        }
        protected virtual void OnTriggerStay(Collider other)
        {
            currentState.TriggerStay(other);
        }
        private void Update()
        {
            if (base.IsOwner)
            {

                currentState.UpdateOwnerState();
               
                if (Input.GetKeyDown(KeyCode.RightAlt))
                {
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.angularVelocity = Vector3.zero;
                }
            }
            if(base.IsServer)
            {
                currentState.UpdateServerState();
            }
        }

        private void PredictionManager_OnPreReplicateReplay(uint arg1, PhysicsScene arg2, PhysicsScene2D arg3)
        {
            /* Server does not replay so it does
             * not need to add gravity. */
            if (!base.IsServer)
                AddGravity();
        }


        private void TimeManager_OnTick()
        {
            if (base.IsOwner)
            {
                Reconciliation(default, false);
                BuildMoveData(out MoveData md);
                Move(md, false);
            }
            if (base.IsServer)
            {
                Move(default, true);
            }

            /* Server and all clients must add the additional gravity.
             * Adding gravity is not necessarily required in general but
             * to make jumps more snappy extra gravity is added per tick.
             * All clients and server need to simulate the gravity to keep
             * prediction equal across the network. */
            AddGravity();
        }

        private void TimeManager_OnPostTick()
        {
            /* Reconcile is sent during PostTick because we
             * want to send the _rigidbody data AFTER the simulation. */
            if (base.IsServer)
            {
                uint localTick = base.TimeManager.LocalTick;

                // we reconcilate at reduce tick step for bandwidth saving!
                //if ((localTick % 20) == 0)
                {
                    ReconcileData rd = new ReconcileData(transform.position, transform.rotation, _rigidbody.velocity, _rigidbody.angularVelocity);
                    Reconciliation(rd, true);
                }

            }
        }

     
        /// <summary>
        /// Adds gravity to the rigidbody.
        /// </summary>
        private void AddGravity()
        {
            //if(!isGrounded)    
           // _rigidbody.AddForce(Physics.gravity * 2f);
        }
       
        /// <summary>
        /// Builds a MoveData to use within replicate.
        /// </summary>
        /// <param name="md"></param>
        public struct MoveData : IReplicateData
        {
            public bool Jump;
            public bool Dash;
            public bool IsGrounded;
            public bool Sprint;
            public bool IsMaxSpeed;

            public bool IsGetHit;
            public bool IsDodge;
            public bool IsStoped;


            public float RootEulerY;
            public float PlayerRotX;
            public float PlayerRotZ;

            public float Horizontal;
            public float Vertical;
            public float MaxSpeed;

            public float JumpForce;
            public float DashForce;

            public MoveData(bool jump,bool dahs, bool isGrounded, bool sprint,bool isMaxSpeed,float playerRotX,float playerRotZ, bool isGetHit, bool isDodge, bool isStoped,float camEulerY, float horizontal, float vertical, float maxSpeed, float jumpForce, float dashForce) { 
                Jump = jump;
                Dash = dahs;
                IsGrounded = isGrounded;
                Sprint = sprint;
                IsMaxSpeed = isMaxSpeed;
                IsGetHit = isGetHit;
                IsDodge = isDodge;
                IsStoped = isStoped;
                

                RootEulerY = camEulerY;

                Horizontal = horizontal;
                Vertical = vertical;
                MaxSpeed = maxSpeed;

                
                PlayerRotX = playerRotX;
                PlayerRotZ = playerRotZ;
                JumpForce = jumpForce;
                DashForce = dashForce;
                _tick = 0;
            }

            private uint _tick;
            public void Dispose() { }
            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
        }
        private void BuildMoveData(out MoveData md)
        {
            md = default;
            bool isJump = currentState.IsJump;
            bool isDash = currentState.IsDash;

            bool isGrounded = currentState.IsGrounded;
            bool sprint = currentState.IsSprint;//Input.GetKey(KeyCode.LeftShift);
            bool isMaxSpeed = _rigidbody.velocity.magnitude > this.maxSpeed;// currentState.IsMaxSpeed;
            bool isGetHit = currentState.IsGetHit;
            bool isDodge = currentState.IsDodge;
            bool isStoped = currentState.IsStoped;

            float rootEulerY = currentState.RootEulerY;

            //float aimY = currentState.AimY;
            //Mathf.SmoothDampAngle(transform.eulerAngles.y, RootTransform.eulerAngles.y, ref turnSmoothVel, turnSmoothTime);
            float horizontal = currentState.Horizontal;//Input.GetAxisRaw("Horizontal");
            float vertical = currentState.Vertical;//Input.GetAxisRaw("Vertical");
;           float maxSpeed = currentState.MaxSpeed;
            float playerRotX = currentState.PlayerRotX;
            float playerRotZ = currentState.PlayerRotZ;
            float jumpForce = currentState.JumpForceValue;
            float dashForce = currentState.DashForce;
            // if (horizontal == 0f && vertical == 0f && !_jump)
            //   return;

            md = new MoveData(isJump, isDash, isGrounded, sprint, isMaxSpeed, playerRotX, playerRotZ, isGetHit, isDodge, isStoped, rootEulerY, horizontal, vertical,maxSpeed, jumpForce, dashForce);
            currentState.IsJump = false;
            currentState.IsSprint = false;
            //currentState.IsDash = false; 
            currentState.IsGetHit = false;
            currentState.IsDodge = false;
            currentState.IsStoped = false;
           
            //isJump = false;
        }
        Quaternion rotationTarget;
        Vector3 locomotionForces;

        [Replicate]
        private void Move(MoveData md, bool asServer, Channel channel = Channel.Unreliable, bool replaying = false)
        {

            if (md.Sprint)
            {
                maxSpeed = 10f;
            }
            else
            {
                maxSpeed = md.MaxSpeed;
            }

            if (md.Dash)
            {
                _rigidbody.velocity = (transform.forward * md.DashForce) + Vector3.up * -9.8f;
            }
            else
            {
                if (!md.IsMaxSpeed)
                {
                    //float delta = (float)base.TimeManager.TickDelta;


                    locomotionForces = (transform.forward * md.Vertical) + (transform.right * md.Horizontal);// * _moveRate;
                                                                                                             // locomotionForces.Normalize();

                    if (!md.IsStoped)
                    {
                        _rigidbody.AddForce(locomotionForces * _moveRate, ForceMode.Force);
                    }
                }

            }
            //   if (md.RootEulerY != 0)
            {
                rotationTarget = Quaternion.Euler(md.PlayerRotX*10, md.RootEulerY, md.PlayerRotZ*10);
                transform.rotation = rotationTarget;

            }
            if (md.Jump)
                _rigidbody.AddForce(new Vector3(0f, md.JumpForce, 0f), ForceMode.Impulse);

          /*  if (md.Dash)
            {
                if (md.IsGetHit)
                {
                    dashDir = -transform.forward;
                }
                else if (md.IsDodge)
                {

                    dashDir = (transform.forward * md.Vertical) + (transform.right * md.Horizontal);// * _moveRate;
                    dashDir.Normalize();
                }
                else
                {
                    dashDir = transform.forward;
                }

                // _rigidbody.AddForce(dashDir * md.DashForce, ForceMode.Impulse);
            }
          */
            if (!md.IsGrounded)
            {
                _rigidbody.AddForce(0, -9.80665f, 0);
            }
            if (md.IsStoped)
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
            }
        }

        [Reconcile]
        private void Reconciliation(ReconcileData rd, bool asServer, Channel channel = Channel.Unreliable)
        {
            transform.position = rd.Position;
            transform.rotation = rd.Rotation;
            _rigidbody.velocity = rd.Velocity;
            _rigidbody.angularVelocity = rd.AngularVelocity;

        }





#endif
    }

}

