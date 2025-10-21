using System;
using Game.Variables;

namespace Game
{
    [Serializable]
    public class StringReference
    {
        public VariablePreference Preference;
        public string ConstantValue;

        public StringVariable Variable;

        public string Value
        {
            get { return Preference == VariablePreference.Constant ? ConstantValue : Variable.Value; }
        }

        public void Set(string value)
        {
            Variable.Set(value);
        }

        public void AddEvent(Action<string> action)
        {
            Variable.AddEvent(action);
        }

        public void RemoveEvent(Action<string> action)
        {
            Variable.RemoveEvent(action);
        }
    }
}