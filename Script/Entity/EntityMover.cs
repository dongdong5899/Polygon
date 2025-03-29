using System;
using YH.Animators;
using UnityEngine;
using Doryu.StatSystem;

namespace YH.Entities
{
    public class EntityMover : MonoBehaviour, IEntityComponent, IAfterInitable
    {
        [SerializeField] private StatElementSO moveSpeedSO;
        private Vector2 _movement;
        [Header("AnimParams")] 
        
        public Vector2 Velocity => _rbCompo.linearVelocity;
        public bool CanManualMove { get; set; } = true; //키보드로 움직임 가능
        public float SpeedMultiplier { get; set; } = 1f;
        
        private Rigidbody2D _rbCompo;
        private Entity _entity;
        private EntityRenderer _renderer;
        private StatCompo _statCompo;
        private StatElement _speedStat;

        
        private Collider2D _collider;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _rbCompo = entity.GetComponent<Rigidbody2D>();
            _renderer = entity.GetCompo<EntityRenderer>();
            _collider = entity.GetComponent<Collider2D>();
            _statCompo = entity.GetCompo<StatCompo>();
        }

        public void AfterInit()
        {
            _speedStat = _statCompo.GetElement(moveSpeedSO);
        }

        public void AddForceToEntity(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse)
        {
            _rbCompo.AddForce(force, mode);
        }

        public void StopImmediately()
        {
            _rbCompo.linearVelocity = Vector2.zero;
            _movement = Vector2.zero;
        }

        public void SetMovement(Vector2 movement) => _movement = movement;
        
        private void FixedUpdate()
        {
            MoveCharacter();
        }

        private void MoveCharacter()
        {
            if(CanManualMove)
                _rbCompo.linearVelocity = _movement * _speedStat.Value;
        }

    }
}
