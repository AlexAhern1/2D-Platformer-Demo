using Game.Input;
using UnityEngine;

namespace Game
{
    public class EditorLevelInitializer : MonoBehaviour, IInitializable, IEnable
    {
        [SerializeField] private InputManager _input;
        [SerializeField] private GameEvent _enableDefaultInputEvent;

        public void Initialize()
        {
            _input.Initialize();
        }

        public void Enable()
        {
            _input.Enable();
            _enableDefaultInputEvent.Raise();
        }

        public void Disable()
        {
            _input.Disable();
        }
    }
}