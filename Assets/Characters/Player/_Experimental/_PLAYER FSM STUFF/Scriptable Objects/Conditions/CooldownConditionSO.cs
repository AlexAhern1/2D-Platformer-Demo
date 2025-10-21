using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Conditions/Cooldown")]
    public class CooldownConditionSO : ConditionSO
    {
        [SerializeField] private float _cooldownDuration; //could change it to a stat in the future.

        //state variables
        [SerializeField][ReadOnly] private float _nextUseTime;

        public override bool Evaluate() => Time.time >= _nextUseTime;

        public override void ResetCondition()
        {
            _nextUseTime = Time.time + _cooldownDuration;
        }

        public void RefreshCooldown() => _nextUseTime = Time.time;

        public override void Initialize()
        {
            _nextUseTime = 0;
        }
    }
}