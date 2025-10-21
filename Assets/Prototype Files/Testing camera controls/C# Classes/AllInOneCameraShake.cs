using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class AllInOneCameraShake : ICameraShaker
    {
        [Min(0)] public float XAmplitude;
        [Min(0)] public float YAmplitude;

        public AnimationCurve DampCurve;
        public bool UseUnscaledTime;

        // state variables
        private float _time;

        public void SetupShake()
        {
            _time = 0;
        }

        public Vector3 GetShakeVelocity()
        {
            _time += UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            return DampCurve.Evaluate(_time) * new Vector2(Random.Range(-XAmplitude, XAmplitude), Random.Range(-YAmplitude, YAmplitude));
        }
    }
}