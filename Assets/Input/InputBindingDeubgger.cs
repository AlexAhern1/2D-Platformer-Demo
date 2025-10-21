using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public class InputBindingDeubgger : MonoBehaviour
    {
        [Header("Input SO Distributor")]
        [SerializeField] private InputSystemDistributor _inputDistributor;

        private InputSystem _inputSystem;

        public void Initialize()
        {
            _inputSystem = _inputDistributor.InputSystem;
        }

        public void Enable()
        {
            //LEVEL input bindings
            Bind(_inputSystem.Level.Move, OnMove);
            Bind(_inputSystem.Level.Attack, OnAttack);
            Bind(_inputSystem.Level.Jump, OnJump);
            Bind(_inputSystem.Level.Dodge, OnDodge);
            Bind(_inputSystem.Level.EnergyBlast, OnEBlast);
            Bind(_inputSystem.Level.PowerDash, OnPDash);
            Bind(_inputSystem.Level.Heal, OnHeal);
            Bind(_inputSystem.Level.OpenInventory, OnInventory);
            Bind(_inputSystem.Level.Pause, OnPause);
            Bind(_inputSystem.Level.Special, OnSpecial);
            Bind(_inputSystem.Level.Module, OnModule);
            Bind(_inputSystem.Level.Interact, OnInteract);
            Bind(_inputSystem.Level.Cycle, OnCycle);

        }

        public void Disable()
        {
            //LEVEL input bindings
            Unbind(_inputSystem.Level.Move, OnMove);
            Unbind(_inputSystem.Level.Attack, OnAttack);
            Unbind(_inputSystem.Level.Jump, OnJump);
            Unbind(_inputSystem.Level.Dodge, OnDodge);
            Unbind(_inputSystem.Level.EnergyBlast, OnEBlast);
            Unbind(_inputSystem.Level.PowerDash, OnPDash);
            Unbind(_inputSystem.Level.Heal, OnHeal);
            Unbind(_inputSystem.Level.OpenInventory, OnInventory);
            Unbind(_inputSystem.Level.Pause, OnPause);
            Unbind(_inputSystem.Level.Special, OnSpecial);
            Unbind(_inputSystem.Level.Module, OnModule);
            Unbind(_inputSystem.Level.Interact, OnInteract);
            Unbind(_inputSystem.Level.Cycle, OnCycle);

        }

        private void Bind(InputAction contextBinding, Action<InputAction.CallbackContext> callback)
        {
            contextBinding.performed += callback;
            contextBinding.canceled += callback;
        }

        private void Unbind(InputAction contextBinding, Action<InputAction.CallbackContext> callback)
        {
            contextBinding.performed -= callback;
            contextBinding.canceled -= callback;
        }

        /// <summary>
        /// <para>Level: walk / run / look up / look down </para>
        /// </summary>
        /// <param name="context"></param>
        private void OnMove(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<Vector2>();
            Logger.Log($"MOVE: {value}", Logger.Controller, MoreColors.Acid);
        }

        private void OnAttack(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            Logger.Log($"MELEE ATTACK: {context.ReadValue<float>()}", Logger.Controller, MoreColors.LightRose);
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            Logger.Log($"JUMP: {value}", Logger.Controller, MoreColors.Emerald);
        }

        private void OnDodge(InputAction.CallbackContext context)
        {
            var value = context.ReadValue<float>();
            Logger.Log($"DODGE: {value}", Logger.Controller, MoreColors.Canary);
        }

        private void OnEBlast(InputAction.CallbackContext context)
        {
            Logger.Log($"E-BLAST/LASER: {context.ReadValue<float>()}", Logger.Controller, MoreColors.BlizzardBlue);
        }

        private void OnPDash(InputAction.CallbackContext context)
        {
            Logger.Log($"P-DASH/LAUNCH: {context.ReadValue<float>()}", Logger.Controller, MoreColors.SkyBlue);
        }

        private void OnHeal(InputAction.CallbackContext context)
        {
            Logger.Log($"HEAL: {context.ReadValue<float>()}", Logger.Controller, MoreColors.Pink);
        }

        private void OnInventory(InputAction.CallbackContext context)
        {
            Logger.Log($"INVENTORY: {context.ReadValue<float>()}", Logger.Controller, MoreColors.White);
        }

        private void OnPause(InputAction.CallbackContext context)
        {
            Logger.Log($"PAUSE: {context.ReadValue<float>()}", Logger.Controller, MoreColors.Gray);
        }

        private void OnSpecial(InputAction.CallbackContext context)
        {
            Logger.Log($"SPECIAL: {context.ReadValue<float>()}", Logger.Controller, MoreColors.Ruby);
        }

        private void OnModule(InputAction.CallbackContext context)
        {
            Logger.Log($"MODULE: {context.ReadValue<float>()}", Logger.Controller, MoreColors.Orange);
        }

        private void OnInteract(InputAction.CallbackContext context)
        {
            Logger.Log($"INTERACT: {context.ReadValue<float>()}", Logger.Controller, MoreColors.Platinum);
        }

        private void OnCycle(InputAction.CallbackContext context)
        {
            Logger.Log($"CYCLE: {context.ReadValue<Vector2>()}", Logger.Controller, MoreColors.Amber);
        }
    }
}