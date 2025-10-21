using System;
using Game.Variables;

namespace Game
{
    [Serializable]
    public class BoolReference
    {
        public VariablePreference Preference;
        public bool ConstantValue;

        public BoolVariable Variable;

        public bool Value
        {
            get { return Preference == VariablePreference.Constant ? ConstantValue : Variable.Value; }
        }

        public void Set(bool value)
        {
            Variable.Set(value);
        }

        public void AddEvent(Action<bool> action)
        {
            Variable.AddEvent(action);
        }

        public void RemoveEvent(Action<bool> action)
        {
            Variable.RemoveEvent(action);
        }
    }
}