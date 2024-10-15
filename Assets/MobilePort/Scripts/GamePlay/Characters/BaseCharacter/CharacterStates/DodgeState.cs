using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CharacterBehaviour
{

    public class DodgeState : CharacterState
    {
        [SerializeField] Transform lerpObject;
        
        [SerializeField] float dodgeTime = .5f;
        [SerializeField] float lerpSource;
        [SerializeField] float dodgeDashForce = 10f;

        [SerializeField] float minDodgeTime = .5f;

        float weight;
        float elapsed;
        
        public override void AnimationEnd()
        {
            
        }

        public override void EndAnimation()
        {
            
        }

        public override void CancelState()
        {
            
        }

        public override void InitState()
        {
            if(IsOwner)
            {
                if(playerInputs.isMobile)
                {
                    playerInputs.isDodge = false;
                }

                playerInputs.UpdateInputs();
                ReconcileStandardMovement();
                characterManager.IsDodge = true;
                characterManager.DashForce = dodgeDashForce;
                characterManager.IsRootMotion = true;
                
            }
            elapsed = 0f;
            
        }

        public override void TriggerEneter(Collider other)
        {
            if (other.CompareTag("Weapon"))
            {
                if(other.TryGetComponent(out Weapon weapon))
                {
                    if (characterManager.CheckHitValidation(weapon))
                    {
                        weight = 1f;
                        rigs.SetRigWeightServer(1f, RigPart.dodge);
                        lerpObject.position = other.transform.position;
                    }
                    else
                    {
                        Debug.Log("Wykrywa moja bron");
                    }
                }
            }
        }
        public override void TriggerExit(Collider other)
        {
           
        }
        public override void TriggerStay(Collider other)
        {
            if (other.CompareTag("Weapon"))
            {
                if (other.TryGetComponent(out Weapon weapon))
                {
                    if (characterManager.CheckHitValidation(weapon))
                    {
                        lerpObject.position = Vector3.Lerp(lerpObject.position, other.transform.position, lerpSource * Time.deltaTime);
                      //  rigs.UpdateSourcePositionServer(lerpObject.position, RigPart.dodge);
                    }
                    else
                    {
                        Debug.Log($"Wykrywa moja bron????{weapon}");
                    }
                } 
            }
        }

        public override void UpdateOwnerState()
        {
            characterAnimations.CalculateDirectionSpeed();
            characterAnimations.UpdateAnimatorParameters();

            elapsed += Time.deltaTime;
            weight -= RigWeight(dodgeTime);
            rigs.SetRigWeightServer(weight, RigPart.dodge);

            if(IsOwner)
            {
                playerInputs.UpdateInputs();
                ReconcileStandardMovement();
            }
            if (weight <= 0f && elapsed >= minDodgeTime)
            {
               // StandardReturn();
            }
            if (IsOwner)
                if (playerInputs.isAttack)
                {
                    if(characterManager.isDistanceFighting)
                    {
                        playerManager.SwitchCurrentState(playerManager.distanceAttackState);
                    }
                    else
                        playerManager.SwitchCurrentState(playerManager.attackState);
                }
        }

        public override void UpdateServerState()
        {
            
        }

        

        
    }
}
