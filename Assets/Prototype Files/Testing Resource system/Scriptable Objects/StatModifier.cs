using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Stats/Modifier")]
    public class StatModifier : ScriptableObject
    {
        public float Modification;
        public ModifierType Type;
        public bool Stackable;
    }

    public enum ModifierType
    {
        Additive,
        FirstPercent,
        SecondPercent
    }
}