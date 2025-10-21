using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    /// <summary>
    /// Enables/Disables/Swaps input action maps
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private InputSystemDistributor _inputSystemDistributor;
        [SerializeField] private InputEventMediator _inputEventMediator;
        [Separator]
        [SerializeField] private string _defaultMap;
        [Separator]
        [SerializeField] private string _levelMapName;
        [SerializeField] private string _menusMapName;

        [Header("Action map swapping events")]
        [SerializeField] private GameEvent _disableCurrentMapEvent;
        [SerializeField] private GameEvent _enableLevelMapEvent;
        [SerializeField] private GameEvent _enableMenusMapEvent;

        private Dictionary<string, InputActionMap> _actionMaps;

        [Header("Initialization")]
        [SerializeField] private bool _selfInitialize;
        [SerializeField] private bool _enableDefaultInputs;

        // current map
        private string _currentMap;

        public void Initialize()
        {
            if (!_enableDefaultInputs) return;
            var maps = _inputSystemDistributor.InputSystem.asset.actionMaps;
            _actionMaps = new(maps.Count);

            for (int i = 0; i < maps.Count; i++)
            {
                var map = maps[i];
                _actionMaps[map.name] = map;

                if (_defaultMap == map.name)
                {
                    _currentMap = map.name;
                    map.Enable();
                }
            }
        }

        public void Enable()
        {
            _inputEventMediator.Activate();

            _disableCurrentMapEvent.AddEvent(OnDisableCurrentMap);
            _enableLevelMapEvent.AddEvent(EnableLevelMap);
            _enableMenusMapEvent.AddEvent(EnableMenusMap);
        }

        public void Disable()
        {
            _inputEventMediator.Deactivate();

            _disableCurrentMapEvent.RemoveEvent(OnDisableCurrentMap);
            _enableLevelMapEvent.RemoveEvent(EnableLevelMap);
            _enableMenusMapEvent.RemoveEvent(EnableMenusMap);
        }

        private void EnableLevelMap()
        {
            if (!_currentMap.Equals("")) _actionMaps[_currentMap].Disable();
            else if (_currentMap.Equals(_levelMapName)) return;
            EnableMap(_levelMapName);
        }

        private void EnableMenusMap()
        {
            if (!_currentMap.Equals("")) _actionMaps[_currentMap].Disable();
            else if (_currentMap.Equals(_menusMapName)) return;
            EnableMap(_menusMapName);
        }

        private void OnDisableCurrentMap()
        {
            if (_currentMap.Equals("")) return;
            _actionMaps[_currentMap].Disable();

            _currentMap = "";
        }

        private void EnableMap(string mapName)
        {
            _currentMap = mapName;
            _actionMaps[mapName].Enable();
        }

        private void Awake()
        {
            if (!_selfInitialize) return;
            Initialize();
        }

        private void OnEnable()
        {
            if (!_selfInitialize) return;
            Enable();
        }

        private void OnDisable()
        {
            if (!_selfInitialize) return;
            Disable();
        }
    }
}