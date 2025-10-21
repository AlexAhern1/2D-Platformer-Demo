using UnityEngine;

namespace Game
{
    public class CameraShakerHandler : MonoBehaviour
    {
        [SerializeField] private CameraController _controller;
        [SerializeField] private ScreenShakeEvent _event;

        private void OnEnable()
        {
            _event.AddEvent(OnReceiveScreenShake);
        }

        private void OnDisable()
        {
            _event.RemoveEvent(OnReceiveScreenShake);
        }

        private void OnReceiveScreenShake(ScreenShakeData shaker)
        {
            _controller.SetCameraShake(shaker.Shaker, shaker.Duration);
        }
    }
}