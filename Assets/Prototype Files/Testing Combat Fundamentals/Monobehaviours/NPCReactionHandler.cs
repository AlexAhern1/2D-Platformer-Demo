using System;
using UnityEngine;

namespace Game
{
    public class NPCReactionHandler : MonoBehaviour
    {
        [SerializeField] private NPCReactionWindow[] _reactionWindows;

        public void SetReactionWindow(int ID, float start, float end) => _reactionWindows[ID].SetTimes(start, end);

        public bool CanReact(int ID) => _reactionWindows[ID].CanReact;
    }

    [Serializable]
    public class NPCReactionWindow
    {
        public string InspectorName;
        [SerializeField][ReadOnly] private float _startTime;
        [SerializeField][ReadOnly] private float _endTime;

        public bool CanReact => !(Time.time < _startTime || Time.time > _endTime);

        public void SetTimes(float start, float end)
        {
            _startTime = Time.time + start;
            _endTime = Time.time + end;
        }
    }
}