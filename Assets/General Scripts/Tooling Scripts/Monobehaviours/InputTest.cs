using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game
{
    public class InputTest : MonoBehaviour
    {
        InputSystem system;

        public UnityEvent testMethod;

        private void Awake()
        {
            system = new InputSystem();
        }

        private void OnEnable()
        {
            system.Enable();
            system.Level.Debug.started += KeyDown;
        }

        private void OnDisable()
        {
            system.Level.Debug.started -= KeyDown;
            system.Disable();
        }

        void KeyDown(InputAction.CallbackContext context)
        {
            testMethod?.Invoke();
        }

        public void Test()
        {
            Debug.Log("ok");
        }
    }
}