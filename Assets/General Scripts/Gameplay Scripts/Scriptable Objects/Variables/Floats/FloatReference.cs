using System;
using Game.Variables;

namespace Game
{
    [Serializable]
    public class FloatReference : IFloatGetter
    {
        public VariablePreference Preference;
        public float ConstantValue;

        public FloatVariable Variable;

        public float Value
        {
            get { return Preference == VariablePreference.Constant ? ConstantValue : Variable.Value; }
        }

        public void Set(float value)
        {
            Variable.Set(value);
        }

        public void AddEvent(Action<float> action)
        {
            Variable.AddEvent(action);
        }

        public void RemoveEvent(Action<float> action)
        {
            Variable.RemoveEvent(action);
        }

        public float GetFloat() => Value;
    }
}