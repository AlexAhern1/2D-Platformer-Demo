using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour, IEnable, IInitializable
    {
        [SerializeField] private StateMachine _stateMachine;
        [SerializeField] private bool _manualStart;

        [Header("Initializable components")]
        [SerializeField]
        private InterfaceReference<MonoBehaviour, IInitializable>[] _initalizables;

        public void Initialize()
        {
            for (int i = 0; i < _initalizables.Length; i++)
            {
                _initalizables[i].Interface.Initialize();
            }
        }

        public void Enable()
        {
            _stateMachine.Enable();
        }

        public void Disable()
        {
            _stateMachine.Disable();
        }

        private void Awake()
        {
            if (_manualStart) Initialize();
        }

        private void OnEnable()
        {
            if (_manualStart) Enable();
        }

        private void OnDisable()
        {
            if (_manualStart) Disable();
        }
    }
}