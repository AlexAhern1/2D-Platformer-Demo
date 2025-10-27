using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour, IInitializable, IEnable
    {
        [SerializeField] private Camera _camera;

        private ICameraController _currentCameraController;

        private ICameraShaker _defaultCameraShaker;
        private ICameraShaker _cameraShaker;

        // camera width and height
        private float _cameraWidth;
        private float _cameraHeight;

        // cached data
        private CameraData _cachedData;

        // camera bounds
        [Header("Dynamic variables")]
        [SerializeField] private Rect _cameraBounds;

        [Header("Change Camera Event")]
        [SerializeField] private ChangeCameraEvent _changeCameraEvent;
        [SerializeField] private GameEvent _useDefaultCameraEvent;
        [SerializeField] private FloatEvent _useDefaultCameraEventWithTransitionDuration;

        [Header("Default Camera")]
        [SerializeField] private CameraChangeSettings _defaultCameraSettings;

        // camera shaking
        private bool _isShaking;
        private float _shakingTime;
        private float _shakeEndTime;

        // lerping between two camera control strategies
        private bool _isLerping;
        private float _lerpTime;
        private float _lerpDuration;
        private float _lerpDurationInverse;

        public void Enable()
        {
            _changeCameraEvent.AddEvent(OnChangeCamera);
            _useDefaultCameraEvent.AddEvent(OnUseDefaultCamera);
            _useDefaultCameraEventWithTransitionDuration.AddEvent(OnUseDefaultCamera);
        }

        public void Disable()
        {
            _changeCameraEvent.RemoveEvent(OnChangeCamera);
            _useDefaultCameraEvent.RemoveEvent(OnUseDefaultCamera);
            _useDefaultCameraEventWithTransitionDuration.RemoveEvent(OnUseDefaultCamera);
        }

        public void Initialize()
        {
            _cachedData = default;

            _defaultCameraShaker = new NoCameraShake();
            _cameraShaker = _defaultCameraShaker;
            _changeCameraEvent.Raise(_defaultCameraSettings);
        }

        public void SetCameraShake(ICameraShaker shaker, float duration)
        {
            _cameraShaker = shaker;
            shaker.SetupShake();

            _isShaking = true;
            _shakingTime = Time.time;
            _shakeEndTime = _shakingTime + duration;
        }

        private void OnChangeCamera(CameraChangeSettings settings)
        {
            _currentCameraController = settings.CameraController;
            float changeDuration = settings.CameraChangeDuration;

            _isLerping = changeDuration > 0;

            if (_isLerping)
            {
                _lerpTime = 0;
                _lerpDuration = changeDuration;
                _lerpDurationInverse = 1f / changeDuration;
            }
            else
            {
                _cachedData = _currentCameraController.UpdateCamera();
                SetCameraData(_cachedData);
                UpdateCameraSize();
            }
        }

        private void OnUseDefaultCamera()
        {
            OnChangeCamera(_defaultCameraSettings);
        }

        private void OnUseDefaultCamera(float transitionSeconds)
        {
            _defaultCameraSettings.CameraChangeDuration =   transitionSeconds;
            OnChangeCamera(_defaultCameraSettings);
        }

        private void Update()
        {
            HandleShaking();
            if (_isLerping) HandleLerping();
            else HandleUpdating();
        }

        private void HandleLerping()
        {
            _lerpTime += Time.deltaTime;
            SetCameraData(CameraData.Lerp(_cachedData, _currentCameraController.UpdateCamera(), Mathf.SmoothStep(0, 1, _lerpTime * _lerpDurationInverse)));

            UpdateCameraSize();

            if (_lerpTime < _lerpDuration) return;

            _cachedData = _currentCameraController.UpdateCamera();
            SetCameraData(_cachedData);

            UpdateCameraSize();

            _isLerping = false;
        }

        private void HandleUpdating()
        {
            _cachedData = _currentCameraController.UpdateCamera();
            SetCameraData(_cachedData);
        }

        private void HandleShaking()
        {
            if (!_isShaking || Time.time < _shakeEndTime) return;

            _isShaking = false;
            _cameraShaker = _defaultCameraShaker;
        }

        private void SetCameraData(CameraData data)
        {
            Vector3 position = CameraData.Confine(data, _cameraBounds, _cameraWidth, _cameraHeight) + _cameraShaker.GetShakeVelocity();

            transform.SetPositionAndRotation(position, data.EulerRotation);
            _camera.orthographicSize = data.Size;
        }

        private void UpdateCameraSize()
        {
            _cameraHeight = _camera.orthographicSize;
            _cameraWidth = _cameraHeight * _camera.aspect;
        }

        private void OnDrawGizmosSelected()
        {
            Vector2 center = _cachedData.Position;

            Vector2 p = _cachedData.Position;
            float x = p.x;
            float y = p.y;

            bool touchingBounds = y + _cameraHeight > _cameraBounds.yMax
                               || y - _cameraHeight < _cameraBounds.yMin
                               || x + _cameraWidth > _cameraBounds.xMax
                               || x - _cameraWidth < _cameraBounds.xMin;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(center + _cameraHeight * Vector2.up, center + _cameraHeight * Vector2.down);
            Gizmos.DrawLine(center + _cameraWidth * Vector2.left, center + _cameraWidth * Vector2.right);

            Gizmos.color = touchingBounds ? Color.red : Color.yellow;
            Gizmos.DrawWireCube(_cameraBounds.center, _cameraBounds.size);
        }
    }
}