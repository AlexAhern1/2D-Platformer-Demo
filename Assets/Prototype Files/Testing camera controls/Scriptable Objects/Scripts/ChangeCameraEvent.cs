using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Game Events/Change Camera")]
    public class ChangeCameraEvent : GameEvent<CameraChangeSettings> { }
}