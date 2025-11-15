using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour, IEnable, IInitializable
    {
        [SerializeField] private bool _manualStart;

        [Header("Initializable components")]
        [SerializeField]
        private InterfaceReference<MonoBehaviour, IInitializable>[] _initalizables;

        [Header("Enable/Disable components")]
        [SerializeField]
        private InterfaceReference<MonoBehaviour, IEnable>[] _enables;

        [SerializeField] private bool _setActiveOnInitialize;

        [Header("Player components")]
        [SerializeField] private PlayerUnstuckHandler _unstuckHandler;

        public void Initialize()
        {
            for (int i = 0; i < _initalizables.Length; i++)
            {
                _initalizables[i].Interface.Initialize();
            }

            if (_setActiveOnInitialize) gameObject.SetActive(true);
        }

        public void Enable()
        {
            for (int i = 0; i < _enables.Length; i++)
            {
                _enables[i].Interface.Enable();
            }

            _unstuckHandler.Enable();
        }

        public void Disable()
        {
            for (int i = 0; i < _enables.Length; i++)
            {
                _enables[i].Interface.Disable();
            }

            _unstuckHandler.Disable();
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