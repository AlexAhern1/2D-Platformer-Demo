using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [System.Serializable]
    public abstract class UIAnimation
    {
        public UIAnimator Animator;
        public float Duration;
        public bool IsFirst;

        public void Play()
        {
            if (IsFirst) Animator.PlayFirst(this);
            else Animator.PlayAdditional(this);

            StartAnimation();
        }

        public virtual void UpdateAnimation() { }

        public virtual void StartAnimation() { }
    }

    [System.Serializable]
    public class StopPlayingAnimation : UIAnimation
    {
        public override void StartAnimation()
        {
            Animator.StopAnimation();
        }
    }

    [System.Serializable]
    public class ScaleAnimation : UIAnimation
    {
        public enum Type { X, Y, Both }

        [Separator]
        [Header("Config")]
        public Type AnimationType;
        public AnimationCurve NormalizedCurve;

        [Header("Components")]
        public RectTransform Transform;

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

            float xScale;
            float yScale;

            float updatedScale = NormalizedCurve.Evaluate(time);

            switch (AnimationType)
            {
                case Type.X:
                    xScale = updatedScale;
                    yScale = Transform.localScale.y;
                    break;

                case Type.Y:
                    xScale = Transform.localScale.x;
                    yScale = updatedScale;
                    break;

                default:
                    xScale = updatedScale;
                    yScale = updatedScale;
                    break;
            }

            Transform.localScale = new Vector2(xScale, yScale);
        }
    }

    [System.Serializable]
    public class ColorShiftAnimation : UIAnimation
    {
        [Header("Config")]
        public Color Color1;
        public Color Color2;

        [Header("Components")]
        public Image[] Images;

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
            for (int i = 0; i < Images.Length; i++)
            {
                Images[i].color = Color.Lerp(Color1, Color2, time);
            }
        }
    }
}