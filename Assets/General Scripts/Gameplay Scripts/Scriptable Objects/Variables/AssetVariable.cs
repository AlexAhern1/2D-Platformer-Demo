using System;
using UnityEngine;

namespace Game.Variables
{
    /// <summary>
    /// Simple variables that are stored in .asset files
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AssetVariable<T> : ScriptableObject
    {
        private event Action<T> E_updateValueEvent;

        public T Value;

        public void Set(T value)
        {
            Value = value;
            InvokeChangeEvent();
        }

        public void AddEvent(Action<T> action)
        {
            E_updateValueEvent += action;
        }

        public void RemoveEvent(Action<T> action)
        {
            E_updateValueEvent -= action;
        }

        protected void InvokeChangeEvent() => E_updateValueEvent?.Invoke(Value);
    }
}