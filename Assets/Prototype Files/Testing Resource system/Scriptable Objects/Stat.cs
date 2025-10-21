using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Stats/Stat")]
    public class Stat : ScriptableObject
    {
        // consts
        private const float PERCENT_FACTOR = 0.0001f; // (1/100)^2, since we are using 2 percentage multipliers.

        // fields
        [SerializeField] private float _baseValue;
        [SerializeField] private List<StatModifier> _modifiers;
        [ReadOnly][SerializeField] private float _modifiedValue; //only for viewing in the inspector.

        // properties
        public float Value => _modifiedValue;

        // events
        private event Action<float> E_valueChangeEvent;

        [ContextMenu("Recalculate Modified Value")]
        public void Recalculate() => RecalculateModifiedValue();

        public void AddOnChangeEvent(Action<float> e) => E_valueChangeEvent += e;
        public void RemoveOnChangeEvent(Action<float> e) => E_valueChangeEvent -= e;

        private void OnValidate()
        {
            RecalculateModifiedValue();
            E_valueChangeEvent?.Invoke(Value);
        }

        public void AddModifier(StatModifier modifier)
        {
            if (_modifiers.Contains(modifier) && !modifier.Stackable) return;
            _modifiers.Add(modifier);

            RecalculateModifiedValue();
            E_valueChangeEvent?.Invoke(Value);
        }

        public void RemoveModifier(StatModifier modifier)
        {
            if (!_modifiers.Contains(modifier)) return;
            _modifiers.Remove(modifier);

            RecalculateModifiedValue();
            E_valueChangeEvent?.Invoke(Value);
        }

        private void RecalculateModifiedValue()
        {
            float additiveTotal = 0;
            float firstPercentTotal = 100;
            float secondPercentTotal = 100;

            for (int i = 0; i < _modifiers.Count; i++)
            {
                var mod = _modifiers[i];
                if (mod == null) continue;

                switch (mod.Type)
                {
                    case ModifierType.Additive: additiveTotal += mod.Modification; break;
                    case ModifierType.FirstPercent: firstPercentTotal += mod.Modification; break;
                    case ModifierType.SecondPercent: secondPercentTotal += mod.Modification; break;
                    default: break;
                }
            }

            _modifiedValue = (_baseValue + additiveTotal) * firstPercentTotal * secondPercentTotal * PERCENT_FACTOR;
        }
    }
}