namespace Game
{
    [System.Serializable]
    public class DefaultCameraController : ICameraController
    {
        public CameraData Data;

        public CameraData UpdateCamera() => Data;
    }
}