using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;

namespace YH.Players
{
    public class PlayerDieState : EntityState
    {
        private readonly int _dissolveHash = Shader.PropertyToID("_Dissolve");

        private Player _player;
        private EntityMover _mover;
        private Material _material;

        private float _dieTime;

        public PlayerDieState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();

            _mover.StopImmediately();

            _material = _player.GetCompo<EntityRenderer>().SpriteRendererList[0].material;
            _dieTime = Time.time;
        }

        public override void Update()
        {
            base.Update();

            if (Time.time - _dieTime < 0.5f)
                _material.SetFloat(_dissolveHash, (Time.time - _dieTime) * 0.5f);
            else
                GameManager.Instance.GameOver();
        }
    }
}