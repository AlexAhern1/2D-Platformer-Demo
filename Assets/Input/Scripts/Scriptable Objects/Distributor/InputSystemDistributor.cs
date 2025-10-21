using UnityEngine;

namespace Game
{
    /// <summary>
    /// Scriptable object containing the input system, which can be passed around to other classes that need to know about input reading.
    /// </summary>
    [CreateAssetMenu(fileName = "Input System Distributor", menuName = "SO/Input/Distributor")]
    public class InputSystemDistributor : ScriptableObject
    {
        // needed script: enable/disable action maps (fixed amount = ok to hard-code)

        //private InputSystem
        private InputSystem _inputSystem;

        public InputSystem InputSystem
        {
            get
            {
                if (_inputSystem != null) return _inputSystem;
                Logger.Log("Instantiated new input system (this should be called only once)");
                _inputSystem = new InputSystem();
                return _inputSystem;
            }
        }
    }
}