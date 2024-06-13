using CharacterBehaviour;
using FishNet.Object;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimations : NetworkBehaviour
{

    Rigidbody _rigidbody;
    Animator _animator;
    CharacterState state;

    [SerializeField] Transform body;

    float speedVelocityZ;
    float speedVelocityX;

    public bool isJump;
    public bool isLanding;
    public bool isSwitch;
    public bool isGetHit;

    public int standardAttackId;
    public int dashAttackId;
    internal int defAttackId;

    float jumpDuration;

    Vector3 lastPos;
    Vector3 speed;
    Quaternion playerToGround;
    [SerializeField] float distance;
    public LayerMask layerMask;

    Ray ray;
    RaycastHit info;
    private float distanceToGround;

    bool isGrounded;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        _animator = GetComponent<Animator>();
        lastPos = body.transform.position;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        
        state = GetComponent<CharacterState>();
        
    }

    
    private void OnAnimatorIK(int layerIndex)
    {
       // if (!isGrounded) return;

        RaycastHit hit;
        Ray ray = new Ray(_animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
        if(Physics.Raycast(ray, out hit,distance+1,layerMask))
        {
            if(hit.transform.tag == "walk")
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);

                Vector3 footPos = hit.point;
                footPos.y += distance;
                _animator.SetIKPosition(AvatarIKGoal.LeftFoot, footPos);
                _animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));
            }
            
        }
       // else if (Physics.Raycast(ray, out hit, distance + 1, layerMask))
        {
       //    _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 0f);
       //     _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 0f);
        }
        
        
         ray = new Ray(_animator.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
        if (Physics.Raycast(ray, out hit, distance + 1, layerMask))
        {
            if (hit.transform.tag == "walk")
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);

                Vector3 footPos = hit.point;
                footPos.y += distance;
                _animator.SetIKPosition(AvatarIKGoal.RightFoot, footPos);
                _animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));

            }
        }
   //     else
        {
          ///  _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 0f);
           // _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 0f);
        }

    }

    float RigWeight(float time)
    {
        var weight = 1f;
        var times = time / Time.deltaTime;
        var result = weight / times;
        return result;
    }

    public void CalculateDirectionSpeed()
    {
        if (lastPos != body.transform.position)
        {
            speed = body.transform.position - lastPos;
            speed /= Time.deltaTime;
            lastPos = body.transform.position;
        }

        speedVelocityZ = Vector3.Dot(speed, body.transform.forward);
        speedVelocityX = Vector3.Dot(speed, body.transform.right);
         //speedVelocityZ = Vector3.Dot(_rigidbody.velocity, _rigidbody.transform.forward);
         //speedVelocityX = Vector3.Dot(_rigidbody.velocity, _rigidbody.transform.right);
    }
    public void AdaptableAnimatorSpeed()
    {

        if (_rigidbody.velocity.magnitude > 0.1f)
            _animator.SetFloat("speedMultipiler", _rigidbody.velocity.magnitude);
        else
        {
            OrginalAnimatorSpeed();
        }
    }
    public void OrginalAnimatorSpeed()
    {
        _animator.SetFloat("speedMultipiler", 1f);
    }
    public float RootMotionUpdate()
    {
        return _animator.GetFloat("mSpeed");
    }

    public void UpdateAnimatorParameters()
    {
       
        _animator.SetFloat("vertical", speedVelocityZ, 0.1f, Time.deltaTime);
        _animator.SetFloat("horizontal", speedVelocityX, 0.1f, Time.deltaTime);

        _animator.SetBool("jump", isJump);

        if (isJump)
        {
            jumpDuration += Time.deltaTime;
            if(jumpDuration> 2f)
            {
                if(isGrounded)
                {
                    isJump = false;
                }
            }
            
        }
      
        _animator.SetBool("switch", isSwitch);
        _animator.SetBool("getHit", isGetHit);

        _animator.SetInteger("standardAttack", standardAttackId);
        _animator.SetInteger("dashAttack", dashAttackId);
        _animator.SetInteger("defAttack", defAttackId);

    }
    private void Update()
    {
        CalculateDirectionSpeed();
        UpdateAnimatorParameters();
    }




    [ServerRpc(RequireOwnership = true)]
    public void AttackIdServer(int id)
    {
        AttackIdObservers(id);
    }
    [ObserversRpc(BufferLast = true, ExcludeOwner = false)]
    public void AttackIdObservers(int id)
    {
        standardAttackId = id;
        //StartCoroutine(ResetAttackId());
    }

    [ServerRpc]
    public void JumpServer()
    {
        JumpObserver();
    }
    [ObserversRpc(BufferLast = true)]
    void JumpObserver()
    {
        isJump = true;
        jumpDuration = 0f;
    }

    IEnumerator ResetAttackId()
    {
        yield return new WaitForEndOfFrame();
        standardAttackId = 0;
    }


    [ServerRpc(RequireOwnership = true)]
    public void AnimatorAttackSpeedServer(float value)
    {
        AnimatorAttackSpeedObservers(value);
    }
    [ObserversRpc(BufferLast = true, ExcludeOwner = false)]
    void AnimatorAttackSpeedObservers(float value)
    {
        _animator.SetFloat("attackSpeed", value);

    }



    [ServerRpc(RequireOwnership = true)]
    public void SwitchServer(bool value)
    {
        AnimatorSpeedObservers(value);
    }
    [ObserversRpc(BufferLast = true, ExcludeOwner = false)]
    void AnimatorSpeedObservers(bool value)
    {
        isSwitch = value;

    }

}
