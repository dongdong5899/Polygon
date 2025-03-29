using YH.Animators;
using YH.Entities;
using YH.FSM;
using UnityEngine;

namespace YH.Players
{
    public class PlayerMoveState : EntityState
    {
        private Player _player;
        private EntityMover _mover;
        public PlayerMoveState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = _player.GetCompo<EntityMover>();
        }

        public override void Update()
        {
            base.Update();
            Vector2 input = _player.PlayerInput.InputDirection.normalized;
            
            _mover.SetMovement(input);

            if (input.magnitude < 0.05f)
            {
                _player.ChangeState(_player.playerFSM[FSMState.Idle]);
            }
        }
    }
}
