using UnityEngine;

namespace Game
{
    public class CutsceneHandler : MonoBehaviour
    {
        // use events to communicate.
        [SerializeField] private BlackScreenUI _blackScreen;
        [SerializeField] private CanvasGroup _cutsceneGroup;

        [Header("Config")]
        [SerializeField][Range(0, 1)] private float _blackScreenAlpha;

        [Header("Events")]
        [SerializeField] private BoolEvent _enableCutsceneEvent;
        [SerializeField] private GameEvent _cutsceneOverEvent;

        [Header("Input Events")]
        [SerializeField] private InputEvent<Vector2> _navigateInputEvent;
        [SerializeField] private InputEvent<float> _selectInputEvent;

        // state variables
        private bool _cutsceneActive;

        private void Awake()
        {
            _cutsceneGroup.alpha = 0;
        }

        private void OnEnable()
        {
            _enableCutsceneEvent.AddEvent(ToggleCutscene);
        }

        private void OnDisable()
        {
            _enableCutsceneEvent.RemoveEvent(ToggleCutscene);
            if (_cutsceneActive)
            {
                _navigateInputEvent.RemoveEvent(OnPressNavigate);
                _selectInputEvent.RemoveEvent(OnPressSelect);
            }
        }

        private void ToggleCutscene(bool toggle)
        {
            if (toggle) EnableTempCutscene();
            else DisableTempCutscene();
        }

        [ContextMenu("Enable Temp Cutscene")]
        public void EnableTempCutscene()
        {
            _cutsceneActive = true;

            _cutsceneGroup.alpha = 1;
            _blackScreen.SetAlpha(_blackScreenAlpha);

            _navigateInputEvent.AddEvent(OnPressNavigate);
            _selectInputEvent.AddEvent(OnPressSelect);
        }

        [ContextMenu("Disable Temp Cutscene")]
        public void DisableTempCutscene()
        {
            _cutsceneActive = false;

            _cutsceneGroup.alpha = 0;
            _blackScreen.SetAlpha(0);

            _navigateInputEvent.RemoveEvent(OnPressNavigate);
            _selectInputEvent.RemoveEvent(OnPressSelect);
        }

        private void OnPressNavigate(Vector2 inputData)
        {
            Logger.Log($"Navigating Cutscene: {inputData}", MoreColors.BrightLilac);
        }

        private void OnPressSelect(float inputData)
        {
            if (inputData == 0) return;

            EndCurrentCutscene();
        }

        private void EndCurrentCutscene()
        {
            DisableTempCutscene();

            _cutsceneOverEvent.Raise();
        }
    }
}