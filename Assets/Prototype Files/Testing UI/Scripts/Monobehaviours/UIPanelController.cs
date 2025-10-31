using UnityEngine;

namespace Game.UI
{
    public class UIPanelController : MonoBehaviour, IEnable
    {
        [Header("Input events")]
        [SerializeField] private Vector2InputEvent _navigateInputEvent;
        [SerializeField] private FloatInputEvent _selectInputEvent;
        [SerializeField] private FloatInputEvent _backInputEvent;
        [Separator]

        [Header("Input config")]
        [SerializeField] private bool _allowReleasing;

        [Header("Control scheme")]
        [SerializeReference, SubclassSelector]
        private UIControlScheme _controlScheme;
        [Separator]

        [Header("Control scheme event")]
        [SerializeField] private UIControlSchemeEvent _controlSchemeEvent;

        public void Enable()
        {
            _controlSchemeEvent.AddEvent(OnUpdateControlScheme);

            _navigateInputEvent.AddEvent(OnPressNavigate);
            _selectInputEvent.AddEvent(OnPressSelect);
            _backInputEvent.AddEvent(OnPressBack);
        }

        public void Disable()
        {
            _controlSchemeEvent.RemoveEvent(OnUpdateControlScheme);

            _navigateInputEvent.RemoveEvent(OnPressNavigate);
            _selectInputEvent.RemoveEvent(OnPressSelect);
            _backInputEvent.RemoveEvent(OnPressBack);
        }

        private void OnPressNavigate(Vector2 inputData)
        {
            if (inputData == Vector2.zero && !_allowReleasing) return;
            _controlScheme?.OnPressNavigate();
        }

        private void OnPressSelect(float inputData)
        {
            if (inputData == 0 && !_allowReleasing) return;
            _controlScheme?.OnPressSelect();
        }

        private void OnPressBack(float inputData)
        {
            if (inputData == 0 &&  !_allowReleasing) return;
            _controlScheme?.OnPressBack();
        }

        private void OnUpdateControlScheme(UIControlScheme scheme)
        {
            _controlScheme = scheme;
        }
    }
}