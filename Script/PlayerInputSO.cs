using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YH.Players
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/PlayerInputSO")]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public event Action DashEvent;
        public event Action InventoryEvent;
        public event Action CircleSkillEvent;
        
        public Vector2 InputDirection { get; private set; }
        public Vector3 MousePos { get; private set; }

        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls= new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            InputDirection = context.ReadValue<Vector2>();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if(context.performed)
                DashEvent?.Invoke();
        }

        public void OnMousePos(InputAction.CallbackContext context)
        {
            MousePos = context.ReadValue<Vector2>();
        }

        public void OnInventory(InputAction.CallbackContext context)
        {
            if (context.performed)
                InventoryEvent?.Invoke();
        }

        public void OnCircleSkill(InputAction.CallbackContext context)
        {
            if (context.performed)
                CircleSkillEvent?.Invoke();
        }
    }
}
