using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEngine.Rendering.DebugUI;
    public enum RigPart { dodge, hit, leftArm, aim, distanceAim, blockRightArm, pelvisBoost }

namespace CharacterBehaviour
{

    public class CharacterAnimationRiging : NetworkBehaviour
    {
        public Rig dodgeRig;
        public Rig hitRig;
        public Rig leftArmRig;
        public Rig meleeAimRig;
        public Rig distanceAimRig;
        public Rig blockRightArmRig;
        public Rig pelivisBoostRig;

        public MultiAimConstraint headCons;
        public MultiAimConstraint spinCons;
        public MultiAimConstraint leftArmCons;
        public MultiAimConstraint rightArmCons;
        public MultiAimConstraint leftLegCons;
        public MultiAimConstraint rightLegCons; 
        
        public Transform hitSource;
        public Transform leftArmSource;
        public Transform aimSource;
        public Transform blockTarget;

        RotCon rot;

        public void InitAimingStyle(bool distance)
        {
            if(distance)
            {
                pelivisBoostRig.weight = 0f;
                meleeAimRig.weight = 0f;

            }
            else
            {
                pelivisBoostRig.weight = 1f;
                meleeAimRig.weight = 1f;
            }
        }
        public void SetRigWightLocal(float value, RigPart type, float time = 0f)
        {
            if (type == RigPart.dodge)
            {
                dodgeRig.weight = value;
            }
            if (type == RigPart.hit)
            {
                hitRig.weight = value;
            }
            if (type == RigPart.leftArm)
            {
                leftArmRig.weight = value;
            }
            if (type == RigPart.aim)
            {
              
                StartCoroutine(LerpValue(meleeAimRig, value, time));

            }
            if (type == RigPart.distanceAim)
            {
                distanceAimRig.weight = value;
            }
            if(type == RigPart.blockRightArm)
            {
                blockRightArmRig.weight = value;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void SetRigWeightServer(float value, RigPart type, float time = .8f)
        {
            SetRigWeightObserver(value, type, time);
        }
        [ObserversRpc(BufferLast = true)]
        public void SetRigWeightObserver(float value, RigPart type, float time)
        {
            if(type == RigPart.dodge)
            {
                dodgeRig.weight = value;
            }
            if (type == RigPart.hit)
            {
                StopAllCoroutines();
                StartCoroutine(LerpValue(hitRig, value, time));
            }
            if(type == RigPart.leftArm)
            {
                leftArmRig.weight = value;
            }
            if(type==RigPart.aim)
            {
                StopAllCoroutines();
                StartCoroutine(LerpValue(meleeAimRig, value, time));

            }
            if( type == RigPart.distanceAim)
            {
                distanceAimRig.weight = value;
            }
            if (type == RigPart.blockRightArm)
            {
                
                StartCoroutine(LerpValue(blockRightArmRig, value, time));
            }
            if(type == RigPart.pelvisBoost)
            {
                pelivisBoostRig.weight = value;
            }
        }


        [ServerRpc(RequireOwnership = false)]
        public void SetBodyWeightServer(float value, BodyPart type, float time = .8f)
        {
            SetBodyWeightObserver(value, type, time);
        }
        [ObserversRpc(BufferLast = true)]
        public void SetBodyWeightObserver(float value, BodyPart type, float time)
        {
            if(type == BodyPart.Head) 
            {
                headCons.weight = value;
            }
            if(type == BodyPart.Spin)
            {
                spinCons.weight = value;
            }
            if (type == BodyPart.ArmLeft)
            {
                leftArmCons.weight = value;
            }
            if(type == BodyPart.ArmRight)
            {
                rightArmCons.weight = value;
            }
            if(type == BodyPart.LegLeft) 
            {
                leftLegCons.weight = value;
            }
            if(type ==BodyPart.LegRight)
            {
                rightLegCons.weight = value;    
            }

        }
        void GetHitInitValues()
        {
                headCons.weight = 0;      
           
                spinCons.weight = 0;
           
                leftArmCons.weight = 0;
           
                rightArmCons.weight = 0;
                      
                leftLegCons.weight = 0;          
         
                rightLegCons.weight = 0;
            
        }
        IEnumerator LerpValue(Rig rig, float to, float time)
        {

            while (rig.weight != to)
            {
                rig.weight = Mathf.Lerp(rig.weight, to, Time.deltaTime / time);
                yield return new WaitForEndOfFrame();
            }
            if (rig == hitRig)
            {
                GetHitInitValues();
            }
        }

        [ServerRpc]
        public void UpdateRotationServer(RigPart part, Quaternion rotation)
        {
            UpdateRotationObserver(part, rotation);
        }
        [ObserversRpc(BufferLast = true, ExcludeOwner = true)]
        public void UpdateRotationObserver(RigPart part, Quaternion rotation)
        {
            if(part == RigPart.aim)
            {
                aimSource.rotation = rotation;
            }
        }


        [ServerRpc]
        public void UpdateSourcePositionServer(Vector3 position, RigPart part)
        {
            FollowSourcePositionObservers(position, part);
        }
        [ObserversRpc(BufferLast = true)]
        public void FollowSourcePositionObservers(Vector3 position, RigPart part)
        {
            if(part == RigPart.distanceAim)
            {
                aimSource.position = position;
            }
            if(part == RigPart.aim)
            {
                aimSource.position = position;
            }

            if(part == RigPart.hit)
            {
                hitSource.position = position;
            }

            if(part == RigPart.blockRightArm)
            {
                
            }
        }


        [ServerRpc]
        public void FollowSourcePositionServer(Transform target, Transform rig, float time)
        {
            FollowSourcePositionObservers(target, rig, time);
        }
        [ObserversRpc(BufferLast = true)]
        public void FollowSourcePositionObservers(Transform taregt, Transform rig, float time)
        {
            StartCoroutine(FollowPosition(taregt, rig, time));
        }


        IEnumerator FollowPosition(Transform target, Transform rig, float time)
        {
            if(target == null)
            {
                yield return null;
            }
            else
            {
                while (time > 0)
                {
                    hitSource.position = target.position;
                    yield return new WaitForEndOfFrame();
                    time -= Time.deltaTime;
                }
            }
        }

        /*  Vector3 startPos = toLerp.position;

          currentLerpTime += Time.deltaTime;

          if (currentLerpTime > .3f)
          {
              currentLerpTime = .3f;
          }

          float t = currentLerpTime / .3f;
          Debug.Log(t);

          */
        // var delta = (float)base.TimeManager.TickDelta;

        // toLerp.position = Vector3.Lerp(toLerp.position, target, delta * 2f);


        // toLerp.position = Vector3.SmoothDamp(toLerp.position, target,ref curremtVellocity,Damp);

    }
}
