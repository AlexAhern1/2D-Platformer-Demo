using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UIAnimator : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private int _maxAnimations;

        // collections
        private List<OngoingUIAnimation> _animations;
        private int _animationIndex;

        public void PlayFirst(UIAnimation firstAnimation)
        {
            if (_animationIndex > 0)
            {
                StopAnimation();
            }

            _animations[0].Setup(firstAnimation);
            _animationIndex = 1;
            enabled = true;
        }

        public void PlayAdditional(UIAnimation additionalAnimation)
        {
            if (_animationIndex < _animations.Count)
            {
                _animations[_animationIndex].Setup(additionalAnimation);
                _animationIndex++;
            }
            else
            {
                Logger.Warn($"Trying to play too many animations in {this}. Increase the max number of animations if more animations need to be played.");
            }
        }

        public void StopAnimation()
        {
            for (int i = 0; i < _animationIndex; i++)
            {
                _animations[i].Clear();
            }

            _animationIndex = 0;
            enabled = false;
        }

        private void Awake()
        {
            _animations = new(_maxAnimations);
            for (int i = 0; i < _maxAnimations; i++)
            {
                _animations.Add(new OngoingUIAnimation());
            }

            _animationIndex = 0;

            enabled = false;
        }

        private void LateUpdate()
        {
            for (int i = 0; i < _animationIndex; i++)
            {
                var ongoingAnim = _animations[i];
                ongoingAnim.Animation.UpdateAnimation();

                if (ongoingAnim.IsFinished)
                {
                    _animationIndex--;
                    _animations[_animationIndex].Clear();
                    if (_animationIndex == 0)
                    {
                        enabled = false;
                    }                    
                }
            }
        }
    }

    public class OngoingUIAnimation
    {
        public UIAnimation Animation;
        public float EndTime;

        public bool IsFinished => Time.time >= EndTime;

        public void Setup(UIAnimation animation)
        {
            Animation = animation;
            EndTime = Time.unscaledTime + animation.Duration;
        }

        public void Clear()
        {
            Animation = null;
            EndTime = 0;
        }
    }
}