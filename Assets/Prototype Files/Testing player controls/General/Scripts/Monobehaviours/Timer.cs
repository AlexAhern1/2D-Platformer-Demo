using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Timer : MonoBehaviour
    {
        private List<TimerData> _timers;

        public Action StartTimer(float waitSeconds, Action waitFinishCallback)
        {
            if (_timers.Count == 0) enabled = true;

            var newTimer = new TimerData(waitSeconds, waitFinishCallback);

            _timers.Add(newTimer);
            _timers.Sort();

            return () => CancelTimer(newTimer);
        }

        private void Awake()
        {
            enabled = false;
            _timers = new(8);
        }

        private void Update()
        {
            if (Time.time < _timers[0].EndTime) return;

            var completedTimer = _timers[0];

            _timers.Remove(completedTimer);

            completedTimer.EndCallback?.Invoke();

            if (_timers.Count == 0) enabled = false;
        }

        private void CancelTimer(TimerData data, bool invokeEndCallback = false)
        {
            if (!_timers.Contains(data)) return;

            _timers.Remove(data);

            if (invokeEndCallback) data.EndCallback?.Invoke();

            if (_timers.Count == 0) enabled = false;
        }
    }

    public class TimerData : IComparable<TimerData>
    {
        public TimerData(float waitTime, Action endCallback)
        {
            EndTime = waitTime + Time.time;
            EndCallback = endCallback;
        }

        public float EndTime;
        public Action EndCallback;

        public int CompareTo(TimerData other)
        {
            return EndTime.CompareTo(other.EndTime);
        }
    }
}