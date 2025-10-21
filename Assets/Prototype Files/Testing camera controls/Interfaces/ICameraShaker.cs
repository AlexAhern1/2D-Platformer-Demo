using UnityEngine;

namespace Game
{
    public interface ICameraShaker
    {
        public void SetupShake();
        public Vector3 GetShakeVelocity();
    }
}