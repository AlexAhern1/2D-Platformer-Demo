using System;

namespace Game
{
    public interface IGameEvent
    {
        public event Action GameEvent;

        public void AddEvent(Action e);

        public void RemoveEvent(Action e);
    }

    public interface IGameEvent<T>
    {
        public event Action<T> GameEvent;

        public void AddEvent(Action<T> e);

        public void RemoveEvent(Action<T> e);
    }
}