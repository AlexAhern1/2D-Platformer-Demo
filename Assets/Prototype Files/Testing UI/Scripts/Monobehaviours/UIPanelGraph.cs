using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UIPanelGraph : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private IVector2Getter _directionGetter;

        // there will be a default selected button when opening the panel for the first time.
        [Separator]
        [Header("Graph config")]
        [SerializeField] private SerializedDictionary<UIObject, UITarget[]> _targetObjects;
        [Separator]

        [Header("Initial config")]
        [SerializeField] private UIObject _currentObject;
        [SerializeField] private bool _initializeOnAwake;

        [Header("Default config")]
        [SerializeField] private UIObject _defaultObject;

        [Header("Math config")]
        [SerializeField] private float _angleTolerance;

        [Header("<color=lime>TESTING</color>")]
        [SerializeField] private Image _img;

        [ContextMenu("TEST THE THING")]
        public void TestThing()
        {
            Logger.Log($"{_img.color}", _img.color);
        }

        private void Awake()
        {
            // verify that the default object is part of the targets dictionary.
            if (!_targetObjects.ContainsKey(_currentObject))
            {
                Logger.Error($"{_currentObject} is not present in the targets dictionary");
                return;
            }

            else if (_initializeOnAwake)
            {
                _currentObject.Initialize();
            }
        }

        public void Initialize()
        {
            _currentObject.Initialize();
        }

        public void UpdateObjects()
        {
            var value = _directionGetter.Get().normalized;
            if (value == Vector2.zero) return;

            var potentialTargets = _targetObjects[_currentObject];

            float minAngle = Mathf.PI;
            UIObject nearestObject = null;

            for (int i = 0; i < potentialTargets.Length; i++)
            {
                var target = potentialTargets[i];
                var dir = target.Direction.normalized;

                float angle = Mathf.Acos(Vector2.Dot(value, dir));

                if (Mathf.Abs(angle) < Mathf.Min(minAngle, _angleTolerance * Mathf.Deg2Rad))
                {
                    nearestObject = target.Target;
                    minAngle = angle;
                }
            }

            if (nearestObject == null) return;

            _currentObject.Exit();
            _currentObject = nearestObject;
            _currentObject.Enter();
        }

        public void SelectCurrentObject()
        {
            _currentObject.Select();
        }

        public void InitializeDefaultObject()
        {
            if (_defaultObject == null) return;

            _currentObject.Exit();
            _currentObject = _defaultObject;
            _currentObject.Initialize();
        }
    }
}