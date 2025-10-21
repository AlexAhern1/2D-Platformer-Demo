using UnityEngine;

namespace Game.UI
{
    [System.Serializable]
    public class CanvasGroupAnimation : UIAnimation
    {
        public CanvasGroup Group;
        public AnimationCurve AlphaCurve;

        // state variables
        private float _startTime;
        private float _durationInverse;

        public override void StartAnimation()
        {
            _startTime = Time.unscaledTime;
            _durationInverse = 1f / Duration;
        }

        public override void UpdateAnimation()
        {
            float time = (Time.unscaledTime - _startTime) * _durationInverse;
            Group.alpha = AlphaCurve.Evaluate(time);
        }
    }
}