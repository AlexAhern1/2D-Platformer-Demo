using UnityEngine;

namespace Game
{
    /// <summary>
    /// This should be the only script that uses the Awake callback when loading a scene for the first time.
    /// </summary>
    
    public class GameInitiator : MonoBehaviour, IInitializable, IEnable
    {
        [Header("Initializable bindigs")]
        [SerializeField] private InterfaceReference<MonoBehaviour, IInitializable>[] _firstInitializableObjects;

        [Header("Enable or Disable bindings")]
        [SerializeField] private InterfaceReference<MonoBehaviour, IEnable>[] _enableOrDisableObjects;

        [Header("Second Initializable bindings")]
        [SerializeField] private InterfaceReference<MonoBehaviour, IInitializable>[] _secondInitializableObjects;

        [Separator]
        [SerializeField] private bool _initializeOnAwakeCallback;
        [SerializeField] private bool _enableOnEnableCallback;
        [SerializeField] private bool _disableOnDisableCallback;
        [SerializeField] private bool _initializeOnStartCallback;

        public void Initialize()
        {
            DoFirstInitialization();
        }

        public void Enable()
        {
            EnableObjects();
        }

        public void Disable()
        {
            DisableObjects();
        }

        private void Awake()
        {
            if (!_initializeOnAwakeCallback) return;
            DoFirstInitialization();
        }

        private void OnEnable()
        {
            if (!_enableOnEnableCallback) return;
            EnableObjects();
        }

        private void OnDisable()
        {
            if (!_disableOnDisableCallback) return;
            DisableObjects();
        }

        private void Start()
        {
            if (!_initializeOnStartCallback) return;
            DoSecondInitialization();
        }

        private void DoFirstInitialization()
        {
            for (int i = 0; i < _firstInitializableObjects.Length; i++) _firstInitializableObjects[i].Interface.Initialize();
        }

        private void DoSecondInitialization()
        {
            for (int i = 0; i < _secondInitializableObjects.Length; i++) _secondInitializableObjects[i].Interface.Initialize();
        }

        private void EnableObjects()
        {
            for (int i = 0; i < _enableOrDisableObjects.Length; i++) _enableOrDisableObjects[i].Interface.Enable();
        }

        private void DisableObjects()
        {
            for (int i = 0; i < _enableOrDisableObjects.Length; i++) _enableOrDisableObjects[i].Interface.Disable();
        }
    }
}