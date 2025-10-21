using System;
using UnityEngine;

namespace Game
{
    public class CooldownsHandler : MonoBehaviour
    {
        [SerializeField] private CooldownTimer[] Cooldowns;

        public void StartCooldown(int ID) => Cooldowns[ID].StartCooldown();

        public void StartCooldown(int ID, float time) => Cooldowns[ID].StartCooldown(time);

        public bool IsReady(int ID) => Cooldowns[ID].Ready;
    }

    [Serializable]
    public class CooldownTimer
    {
        public float Duration;

        [SerializeField][ReadOnly] private float _nextUseTime;

        public bool Ready => Time.time >= _nextUseTime;

        public void StartCooldown() => _nextUseTime = Time.time + Duration;

        public void StartCooldown(float time) => _nextUseTime = Time.time + time;
    }
}