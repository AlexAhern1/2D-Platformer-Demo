using UnityEngine;

namespace Game.Player
{
    public class PlayerStaticHandler : MonoBehaviour, IEnable
    {
        [Header("Static Events")]
        [SerializeField] private FloatEvent _addStaticEvent;
        [SerializeField] private GameEvent _resetStaticEvent;

        [Header("Static Data Holders")]
        [SerializeField] private ResourceSO _currentStatic;
        [SerializeField] private Stat _maxStaticStat;

        public void Enable()
        {
            _addStaticEvent.AddEvent(OnAddStatic);
            _resetStaticEvent.AddEvent(OnResetStatic);
        }

        public void Disable()
        {
            _addStaticEvent.RemoveEvent(OnAddStatic);
            _resetStaticEvent.RemoveEvent(OnResetStatic);
        }

        private void OnAddStatic(float amount)
        {
            _currentStatic.Add(amount);
        }

        private void OnResetStatic()
        {
            _currentStatic.Set(_maxStaticStat.Value);
        }
    }
}