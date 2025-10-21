using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class BuildConsole : MonoBehaviour
    {
        [SerializeField] private TMP_Text _consoleText;

        [SerializeField] private Button _toggleButton;
        [SerializeField] private TMP_Text _toggleText;

        [SerializeField] private Button _clearButton;

        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _warningColor;
        [SerializeField] private Color _errorColor;

        [SerializeField] private int _lineLimit;

        [SerializeField] private CanvasGroup _group;

        [SerializeField] private bool _selfInitialize;

        private int _lines;

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

        public void Enable()
        {
            Application.logMessageReceived += OnDebugLog;
            Logger.Log("Build Console Enabled");

            _toggleButton.onClick.AddListener(OnToggle);
            _clearButton.onClick.AddListener(OnClear);
        }

        public void Disable()
        {
            Logger.Log("Build Console Disabled");
            Application.logMessageReceived -= OnDebugLog;

            _toggleButton.onClick.RemoveListener(OnToggle);
            _clearButton.onClick.RemoveListener(OnClear);
        }

        private void OnDebugLog(string msg, string stackTrace, LogType type)
        {
            _lines++;
            _consoleText.color = GetColor(type);

            if (_lines < _lineLimit)
            {
                _consoleText.text += $"{msg}\n";
            }
            else
            {
                // get the index of the first \n in the string, then use the substring starting from that index + 1.
                int indentIndex = _consoleText.text.IndexOf('\n');
                _consoleText.text = $"{_consoleText.text[(indentIndex + 1)..]}{msg}\n";
            }

            
        }

        private void OnToggle()
        {
            _group.alpha = _group.alpha == 1 ? 0 : 1;
            _toggleText.text = _group.alpha == 1 ? "Hide Console" : "Show Console";
        }

        private void OnClear()
        {
            _consoleText.text = "";
            _lines = 0;
        }


        private Color GetColor(LogType type)
        {
            switch (type)
            {
                case LogType.Error: return _errorColor;
                case LogType.Warning: return _warningColor;
                default: return _defaultColor;
            }
        }
    }
}