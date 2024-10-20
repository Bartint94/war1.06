using FishNet;
using FishNet.Object;
using FishNet.Object.Prediction;
using FishNet.Transporting;
using UnityEngine;

namespace CharacterBehaviour
{
    public class PlayerNetworkManager : NetworkBehaviour
    {
        PlayerManager playerManager;
        [SerializeField] float deltaMultipiler;

        void Awake()
        {
            InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;
            InstanceFinder.TimeManager.OnPostTick += TimeManager_OnPostTick;
            playerManager = GetComponent<PlayerManager>();
        }

        public override void OnStartClient()
        {
            base.PredictionManager.OnPreReplicateReplay += PredictionManager_OnPreReplicateReplay;
            playerManager.camTransform.gameObject.SetActive(IsOwner);
            playerManager.camTransform.SetParent(null, true);
            playerManager.attackState.enabled = IsOwner;
        }
     


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

        void OnDestroy()
        {
            if (InstanceFinder.TimeManager != null)
            {
                InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
                InstanceFinder.TimeManager.OnPostTick -= TimeManager_OnPostTick;
            }
        }

        public override void OnStopClient()
        {
            base.PredictionManager.OnPreReplicateReplay -= PredictionManager_OnPreReplicateReplay;
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
                    ReconcileData rd = new ReconcileData(transform.position, transform.rotation, playerManager._rigidbody.velocity, playerManager._rigidbody.angularVelocity);
                    Reconciliation(rd, true);
                }

            }
        }
        void AddGravity()
        {

        }
     
       
        /// <summary>
        /// Builds a MoveData to use within replicate.
        /// </summary>
        /// <param name="md"></param>
        public struct MoveData : IReplicateData
        {
            public bool Jump;
            public bool IsRootMotion;
            public bool IsGrounded;
            public bool Sprint;
            public bool IsMaxSpeed;

            public bool IsGetHit;
            public bool IsDodge;
            public bool IsStoped;
            public bool IsProjectile;


            public float RootEulerY;
            public float PlayerRotX;
            public float PlayerRotZ;

            public float Horizontal;
            public float Vertical;
            public float MaxSpeed;

            public float JumpForce;
            public float DashForce;

            public Vector3 RootMotionDirection;
           
            public MoveData
                (
                bool isJump, 
                bool isDash, 
                bool isGrounded, 
                bool sprint, 
                bool isMaxSpeed, 
                float playerRotX,
                float playerRotZ,
                bool isGetHit,
                bool isDodge,
                bool isStoped,
                float rootEulerY,
                float horizontal,
                float vertical,
                float maxSpeed,
                float jumpForce,
                float dashForce,
                Vector3 rootMotionDirection
                )
                : this()
            {
                Jump = isJump;
                IsRootMotion = isDash;
                IsGrounded = isGrounded;
                Sprint = sprint;
                IsMaxSpeed = isMaxSpeed;
                PlayerRotX = playerRotX;
                PlayerRotZ = playerRotZ;
                IsGetHit = isGetHit;
                IsDodge = isDodge;
                IsStoped = isStoped;
                RootEulerY = rootEulerY;
                Horizontal = horizontal;
                Vertical = vertical;
                MaxSpeed = maxSpeed;
                JumpForce = jumpForce;
                DashForce = dashForce;
                RootMotionDirection = rootMotionDirection;
            }

            private uint _tick;
           

            public void Dispose() { }
            public uint GetTick() => _tick;
            public void SetTick(uint value) => _tick = value;
        }
        private void BuildMoveData(out MoveData md)
        {
            md = default;
            bool isJump = playerManager.IsJump;
            bool isRoot = playerManager.IsRootMotion;

            bool isGrounded = playerManager.IsGrounded;
            bool sprint = playerManager.IsSprint;//Input.GetKey(KeyCode.LeftShift);
            bool isMaxSpeed = playerManager._rigidbody.velocity.magnitude > playerManager.maxSpeed;// currentState.IsMaxSpeed;
            bool isGetHit = playerManager.IsGetHit;
            bool isDodge = playerManager.IsDodge;
            bool isStoped = playerManager.IsStoped;



            float rootEulerY = playerManager.RootEulerY;


            float horizontal = playerManager.Horizontal;
            float vertical = playerManager.Vertical;
;           float maxSpeed = playerManager.MaxSpeed;
            float playerRotX = playerManager.PlayerRotX;
            float playerRotZ = playerManager.PlayerRotZ;
            float jumpForce = playerManager.JumpForceValue;
            float dashForce = playerManager.DashForce;
            Vector3 rootMotionDirection = playerManager.RootMotionDirection;


            // if (horizontal == 0f && vertical == 0f && !_jump)
            //   return;

            md = new MoveData(isJump, isRoot, isGrounded, sprint, isMaxSpeed, playerRotX, playerRotZ, isGetHit, isDodge, isStoped, rootEulerY, horizontal, vertical,maxSpeed, jumpForce, dashForce, rootMotionDirection);
            playerManager.IsJump = false;
            playerManager.IsSprint = false;
            playerManager.IsGetHit = false;
            playerManager.IsDodge = false;
            playerManager.IsStoped = false;
            playerManager.IsProjectile = false;
           
            //isJump = false;
        }
        Quaternion rotationTarget;
        Vector3 locomotionForces;

        [Replicate]
        private void Move(MoveData md, bool asServer, Channel channel = Channel.Unreliable, bool replaying = false)
        {
           // if(md.IsProjectile)
            {
                //distanceAttackState.arrow.ShotObserver(md.AimPos, md.AimRot);
            }

            float delta = (float)base.TimeManager.TickDelta;
            
            if (md.IsRootMotion)
            {
                //transform.position += playerManager.animations.RootMotionUpdate();
                playerManager._rigidbody.velocity = (md.RootMotionDirection * md.DashForce) + Vector3.up * deltaMultipiler;//playerManager.animations.RootMotionUpdate() * deltaMultipiler);
                if (!md.IsGrounded)
                {
                    //playerManager._rigidbody.velocity = Vector3.up * Mathf.Lerp(0, -9.8f, delta * deltaMultipiler);
                    
                }
                
            }
            else
            {
                if (!md.IsMaxSpeed)
                {


                    locomotionForces = (transform.forward * md.Vertical) + (transform.right * md.Horizontal*.7f);// * moveRate;
                                                                                                             // locomotionForces.Normalize();

                    if (!md.IsStoped)
                    {
                        playerManager._rigidbody.AddForce(locomotionForces * playerManager.moveRate, ForceMode.Force);
                    }
                }

            }
            
            // PLAYER ROTATIONS
            if(md.RootEulerY != 0)
            playerManager.playerRotY = md.RootEulerY;
            
            rotationTarget = Quaternion.Euler(md.PlayerRotX*10, playerManager.playerRotY, md.PlayerRotZ*10);
            transform.rotation = rotationTarget;

            //JUMP
            if (md.Jump)
                playerManager._rigidbody.AddForce(new Vector3(0f, md.JumpForce, 0f), ForceMode.Impulse);

         
            if (!md.IsGrounded)
            {
                playerManager._rigidbody.AddForce(0, -9.80665f, 0);
            }
            if (md.IsStoped)
            {
                playerManager._rigidbody.velocity = Vector3.zero;
                playerManager._rigidbody.angularVelocity = Vector3.zero;
            }
        }

        [Reconcile]
        private void Reconciliation(ReconcileData rd, bool asServer, Channel channel = Channel.Unreliable)
        {
            transform.position = rd.Position;
            transform.rotation = rd.Rotation;
            playerManager._rigidbody.velocity = rd.Velocity;
            playerManager._rigidbody.angularVelocity = rd.AngularVelocity;

        }
#endif
    }

}

