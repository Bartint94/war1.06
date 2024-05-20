using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CharacterBehaviour
{
    public class EnemyStateManager : CharacterStateManager
    {
        [SerializeField] Transform sourceObject;
        public EnemyTarget enemyTarget;

        public Transform targetTransform;

        public float move;

        [SerializeField] float _moveRate = 11f;
        [SerializeField] float maxMoveVelocity = 13f;

        public EnemyAttackState attackState;

        public bool isDash;
        public Vector3 dashDir;

        public static event Action OnDeath;
        protected override void Awake()
        {
            base.Awake();
            SwitchCurrentState(standardState);
        }
       public void Death()
        {
            OnDeath?.Invoke();
        }
        private void Init()
        {
            //inve
        }

        void Update()
        {
            currentState.UpdateOwnerState();


            Vector3 forces = (transform.forward * move);
            _rigidbody.AddForce(0, -9.80665f, 0);
            if (_rigidbody.velocity.magnitude < maxMoveVelocity)
            {
                _rigidbody.AddForce(forces * _moveRate);
                
            }
            if(isDash)
            {
                _rigidbody.AddForce(dashDir * 10f,ForceMode.Impulse);
                isDash = false;
            }
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



    }
}
