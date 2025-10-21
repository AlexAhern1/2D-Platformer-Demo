using System;
using Game.Variables;

namespace Game
{
    [Serializable]
    public class IntReference
    {
        public VariablePreference Preference;
        public int ConstantValue;

        public IntVariable Variable;

        public int Value
        {
            get { return Preference == VariablePreference.Constant ? ConstantValue : Variable.Value; }
        }

        public void Set(int value)
        {
            Variable.Set(value);
        }

        public void AddEvent(Action<int> action)
        {
            Variable.AddEvent(action);
        }

        public void RemoveEvent(Action<int> action)
        {
            Variable.RemoveEvent(action);
        }
    }
}