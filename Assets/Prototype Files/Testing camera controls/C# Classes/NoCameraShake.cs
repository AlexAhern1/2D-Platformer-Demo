using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class NoCameraShake : ICameraShaker
    {
        public void SetupShake() { }

        public Vector3 GetShakeVelocity() => Vector3.zero;
    }
}