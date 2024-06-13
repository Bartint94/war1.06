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




        public EnemyAttackState attackState;

        public bool isDash;
        public Vector3 dashDir;

        public static event Action OnDeath;
        protected override void Awake()
        {
            base.Awake();
            SwitchCurrentState(standardState);
        }
        public override void SwitchCurrentState(CharacterState state, string debug = "charscter")
        {
            if(IsServer)
            base.SwitchCurrentState(state, debug);
        }
        public void Death()
        {
            OnDeath?.Invoke();
        }


        void Update()
        {
            if(IsServer)
            currentState.UpdateOwnerState();

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
