using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    /// <summary>
    /// <para>This class acts as a bridge between the input system and the various input events,</para>
    /// <para>responsible for listening for inputs and then updating and invoking corresponding</para>
    /// input events.
    /// </summary>
    [Serializable]
    public class InputEventMediator
    {
        [Header("Input system distributor")]
        [SerializeField] private InputSystemDistributor _inputDistributor;

        [Header("Input SO events")]
        [Separator]
        [Header("Level")]
        [SerializeField] private InputEvent<Vector2> _moveInputEvent;
        [SerializeField] private InputEvent<Vector2> _cycleInputEvent;
        [SerializeField] private InputEvent<float> _jumpInputEvent;
        [SerializeField] private InputEvent<float> _dodgeInputEvent;
        [SerializeField] private InputEvent<float> _attackInputEvent;
        [SerializeField] private InputEvent<float> _staticAttack1InputEvent;
        [SerializeField] private InputEvent<float> _staticAttack2InputEvent;
        [SerializeField] private InputEvent<float> _specialAttackInputEvent;
        [SerializeField] private InputEvent<float> _moduleSkillInputEvent;
        [SerializeField] private InputEvent<float> _healInputEvent;
        [SerializeField] private InputEvent<float> _interractInputEvent;
        [SerializeField] private InputEvent<float> _inventoryInputEvent;
        [SerializeField] private InputEvent<float> _pauseInputEvent;
        [SerializeField] private InputEvent<float> _unstuckInputEvent;

        [Header("Menus")]
        [SerializeField] private InputEvent<Vector2> _navigateInputEvent;
        [SerializeField] private InputEvent<float> _selectInputEvent;
        [SerializeField] private InputEvent<float> _backInputEvent;
        [SerializeField] private InputEvent<float> _closeInputEvent;

        [Header("Title")]
        [SerializeField] private InputEvent<float> _anyInputEvent;

        public void Activate()
        {
            var system = _inputDistributor.InputSystem;

            var level = system.Level;
            var menus = system.Menus;
            var title = system.Title;

            Bind(level.Move, OnPressMove);
            Bind(level.Cycle, OnPressCycle);
            Bind(level.Jump, OnPressJump);
            Bind(level.Dodge, OnPressDodge);
            Bind(level.Attack, OnPressAttack);
            Bind(level.EnergyBlast, OnPressStatic1);
            Bind(level.PowerDash, OnPressStatic2);
            Bind(level.Special, OnPressSpecial);
            Bind(level.Module, OnPressModule);
            Bind(level.Heal, OnPressHeal);
            Bind(level.Interact, OnPressInteract);
            Bind(level.OpenInventory, OnPressInventory);
            Bind(level.Pause, OnPressPause);
            Bind(level.Unstuck, OnPressUnstuck);

            Bind(menus.Navigate, OnPressNavigate);
            Bind(menus.Select, OnPressSelect);
            Bind(menus.Back, OnPressBack);
            Bind(menus.Close, OnPressClose);

            Bind(title.Any, OnPressAny);
        }

        public void Deactivate()
        {
            var system = _inputDistributor.InputSystem;

            var level = system.Level;
            var menus = system.Menus;
            var title = system.Title;

            Unbind(level.Move, OnPressMove);
            Unbind(level.Cycle, OnPressCycle);
            Unbind(level.Jump, OnPressJump);
            Unbind(level.Dodge, OnPressDodge);
            Unbind(level.Attack, OnPressAttack);
            Unbind(level.EnergyBlast, OnPressStatic1);
            Unbind(level.PowerDash, OnPressStatic2);
            Unbind(level.Special, OnPressSpecial);
            Unbind(level.Module, OnPressModule);
            Unbind(level.Heal, OnPressHeal);
            Unbind(level.Interact, OnPressInteract);
            Unbind(level.OpenInventory, OnPressInventory);
            Unbind(level.Pause, OnPressPause);
            Unbind(level.Unstuck, OnPressUnstuck);

            Unbind(menus.Navigate, OnPressNavigate);
            Unbind(menus.Select, OnPressSelect);
            Unbind(menus.Back, OnPressBack);
            Unbind(menus.Close, OnPressClose);

            Unbind(title.Any, OnPressAny);
        }

        private void Bind(InputAction inputEvent, Action<InputAction.CallbackContext> inputCallback)
        {
            inputEvent.performed += inputCallback;
            inputEvent.canceled += inputCallback;
        }

        private void Unbind(InputAction inputEvent, Action<InputAction.CallbackContext> inputCallback)
        {
            inputEvent.performed -= inputCallback;
            inputEvent.canceled -= inputCallback;
        }

        #region Level

        private void OnPressMove(InputAction.CallbackContext context) => Process(context.ReadValue<Vector2>(), _moveInputEvent);

        private void OnPressCycle(InputAction.CallbackContext context) => Process(context.ReadValue<Vector2>(), _cycleInputEvent);

        private void OnPressJump(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _jumpInputEvent);

        private void OnPressDodge(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _dodgeInputEvent);

        private void OnPressAttack(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _attackInputEvent);

        private void OnPressStatic1(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _staticAttack1InputEvent);

        private void OnPressStatic2(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _staticAttack2InputEvent);

        private void OnPressSpecial(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _specialAttackInputEvent);

        private void OnPressModule(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _moduleSkillInputEvent);

        private void OnPressHeal(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _healInputEvent);

        private void OnPressInteract(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _interractInputEvent);

        private void OnPressInventory(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _inventoryInputEvent);

        private void OnPressPause(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _pauseInputEvent);

        private void OnPressUnstuck(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _unstuckInputEvent);
        #endregion

        #region Menus

        private void OnPressNavigate(InputAction.CallbackContext context) => Process(context.ReadValue<Vector2>(), _navigateInputEvent);

        private void OnPressSelect(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _selectInputEvent);

        private void OnPressBack(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _backInputEvent);

        private void OnPressClose(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _closeInputEvent);

        #endregion

        #region Title

        private void OnPressAny(InputAction.CallbackContext context) => Process(context.ReadValue<float>(), _anyInputEvent);

        #endregion

        private void Process<T>(T value, InputEvent<T> inputEvent)
        {
            inputEvent.Raise(value);
        }
    }
}