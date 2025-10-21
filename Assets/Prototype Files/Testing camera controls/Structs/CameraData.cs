using UnityEngine;

namespace Game
{
    [System.Serializable]
    public struct CameraData
    {
        private const float Z_DISTANCE = -10;

        public Vector2 Position;
        public float Angle;
        public float Size;

        public CameraData(float x, float y, float angle, float size)
        {
            Position = new Vector2(x, y);
            Angle = angle;
            Size = size;
        }

        public readonly Quaternion EulerRotation => Quaternion.Euler(0, 0, Angle);

        public static CameraData Lerp(CameraData a, CameraData b, float t)
        {
            return new CameraData
            {
                Position = Vector2.Lerp(a.Position, b.Position, t),
                Angle = Mathf.Lerp(a.Angle, b.Angle, t),
                Size = Mathf.Lerp(a.Size, b.Size, t)
            };
        }

        public static CameraData operator +(CameraData a, CameraData b)
        {
            return new CameraData
            {
                Position = a.Position + b.Position,
                Angle = (a.Angle + b.Angle) % 360,
                Size = 0.5f * (a.Size + b.Size),
            };
        }

        public static Vector3 Confine(CameraData data, Rect bounds, float camWidth, float camHeight)
        {
            float x = (bounds.xMax - camWidth < bounds.xMin + camWidth) ? bounds.center.x : Mathf.Clamp(data.Position.x, bounds.xMin + camWidth, bounds.xMax - camWidth);
            float y = (bounds.yMax - camHeight < bounds.yMin + camHeight) ? bounds.center.y : Mathf.Clamp(data.Position.y, bounds.yMin + camHeight, bounds.yMax - camHeight);

            return new(x, y, Z_DISTANCE);
        }
    }
}