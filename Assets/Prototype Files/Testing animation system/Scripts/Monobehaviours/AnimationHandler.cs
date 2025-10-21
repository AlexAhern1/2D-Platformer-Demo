using UnityEngine;

namespace Game
{
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator _animator;



        public void Play(AnimationContext context)
        {
            string animationName = context.Clip.name;
            if (!_animator.HasState(0, Animator.StringToHash(animationName)))
            {
                Logger.Error($"{gameObject}'s Animator does not have an animation named {animationName}!", MoreColors.LightRose);
                return;
            }

            _animator.Play(context.Clip.name);
        }

        public float PlayAndGetDuration(AnimationContext context)
        {
            return 0;
        }

        public void Stop()
        {
            _animator.StopPlayback();
            Logger.Log("stopped an animation.");
        }
    }
}