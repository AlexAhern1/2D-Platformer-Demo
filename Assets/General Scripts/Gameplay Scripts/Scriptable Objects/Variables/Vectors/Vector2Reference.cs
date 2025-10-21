using Game.Variables;
using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class Vector2Reference
    {
        public VariablePreference Preference;
        public Vector2 ConstantValue;

        public Vector2Variable Variable;

        public Vector2 Value
        {
            get { return Preference == VariablePreference.Constant ? ConstantValue : Variable.Value; }
        }

        public void Set(Vector2 value)
        {
            Variable.Set(value);
        }

        public void SetX(float x) => Variable.Set(new Vector2(x, Value.y));

        public void SetY(float y) => Variable.Set(new Vector2(Value.x, y));

        public void AddEvent(Action<Vector2> action)
        {
            Variable.AddEvent(action);
        }

        public void RemoveEvent(Action<Vector2> action)
        {
            Variable.RemoveEvent(action);
        }
    }
}