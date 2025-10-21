using System;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Game Event", menuName = "SO/Game Events/Normal")]
    public class GameEvent : ScriptableObject
    {
        private event Action _eventAction;

        public void AddEvent(Action e) => _eventAction += e;

        public void RemoveEvent(Action e) => _eventAction -= e;

        public void Raise() => _eventAction?.Invoke();
    }

    public abstract class GameEvent<T> : ScriptableObject
    {
        protected event Action<T> EventAction;

        public void AddEvent(Action<T> e) => EventAction += e;

        public void RemoveEvent(Action<T> e) => EventAction -= e;

        public virtual void Raise(T value) => EventAction?.Invoke(value);
    }
}