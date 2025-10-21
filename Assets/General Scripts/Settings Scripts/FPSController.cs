using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class FPSController : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Button _unlimitedButton;
        [SerializeField] private bool _allowUnlimited = true;
        [SerializeField] private int _maxFPS = 240;
        [SerializeField] private int _minFPS = 30;
        [SerializeField] private TMP_Text _targetFPSText;

        [SerializeField] private Image _unlimitedCheckboxImage;

        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(OnSlide);
            _unlimitedButton.onClick.AddListener(ToggleUnlimitedFPS);

            _slider.value = 1;
            OnSlide(1);

            Application.targetFrameRate = -1;
        }

        private void OnDisable()
        {
            _slider.onValueChanged.RemoveListener(OnSlide);
            _unlimitedButton.onClick.RemoveListener(ToggleUnlimitedFPS);

            Application.targetFrameRate = -1;
        }

        private void OnSlide(float value)
        {
            value = Mathf.Clamp01(value);

            if (value == 1 && _allowUnlimited)
            {
                Application.targetFrameRate = -1;
                _targetFPSText.text = "Target: unlimited";
            }
            else
            {
                int fps = Mathf.RoundToInt(Mathf.Lerp(_minFPS, _maxFPS, value));
                Application.targetFrameRate = fps;
                _targetFPSText.text = $"Target: {fps}";
            }
        }

        public void ToggleUnlimitedFPS()
        {
            _allowUnlimited = !_allowUnlimited;
            _unlimitedCheckboxImage.color = _allowUnlimited ? Color.green : Color.black;

            if (_slider.value == 1)
            {
                if (_allowUnlimited)
                {
                    Application.targetFrameRate = -1;
                    _targetFPSText.text = "Target: unlimited";
                }
                else
                {
                    int fps = Mathf.RoundToInt(Mathf.Lerp(_minFPS, _maxFPS, _slider.value));
                    Application.targetFrameRate = fps;
                    _targetFPSText.text = $"Target: {fps}";
                }
            }
        }
    }
}