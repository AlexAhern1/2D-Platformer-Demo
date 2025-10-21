namespace Game
{
    [System.Serializable]
    public class SODataCameraController : ICameraController
    {
        public Vector2Reference PositionReference;
        public float Angle;
        public float Size;

        public CameraData UpdateCamera()
        {
            var pos = PositionReference.Value;
            return new(pos.x, pos.y, Angle, Size);
        }
    }
}