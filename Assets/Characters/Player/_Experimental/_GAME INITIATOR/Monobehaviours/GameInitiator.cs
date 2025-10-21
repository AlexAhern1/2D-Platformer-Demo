using UnityEngine;

namespace Game
{
    /// <summary>
    /// This should be the only script that uses the Awake callback when loading a scene for the first time.
    /// </summary>
    
    public class GameInitiator : MonoBehaviour
    {
        [Header("Initializable bindigs")]
        [SerializeField] private InterfaceReference<GameObject, IInitializable>[] _initializableObjects;

        [Header("Enable or Disable bindings")]
        [SerializeField] private InterfaceReference<GameObject, IEnable>[] _enableOrDisableObjects;

        private void Awake()
        {
            InitializeObjects();
        }

        private void OnEnable()
        {
            EnableObjects();
        }

        private void OnDisable()
        {
            DisableObjects();
        }

        private void InitializeObjects()
        {
            for (int i = 0; i < _initializableObjects.Length; i++) _initializableObjects[i].Interface.Initialize();
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