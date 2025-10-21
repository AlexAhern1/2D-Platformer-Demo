using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class FollowPlayerCameraController : ICameraController
    {
        [Header("Non-position data")]
        public float Angle;
        public float Size;

        public Transform HorizontalFollow;
        public Transform VerticalFollow;

        public CameraData UpdateCamera()
        {
            return new CameraData(HorizontalFollow.position.x, VerticalFollow.position.y, Angle, Size);
        }
    }
}