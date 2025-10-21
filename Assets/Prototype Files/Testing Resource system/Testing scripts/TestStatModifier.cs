using UnityEngine;

namespace Game
{
    public class TestStatModifier : MonoBehaviour
    {
        // state variable
        private bool _modified;
        [SerializeField] private Stat _stat;
        [SerializeField] private StatModifier[] _modifiers;

        public void ToggleModifiers()
        {
            if (_modified)
            {
                OnEquipModifier();
            }
            else
            {
                OnUnequipModifier();
            }

            _modified = !_modified;
        }

        private void OnEquipModifier()
        {
            // apply modifiers
            for (int i = 0; i < _modifiers.Length; i++)
            {
                _stat.AddModifier(_modifiers[i]);
            }
        }

        private void OnUnequipModifier()
        {
            // remove modifiers
            for (int i = 0; i < _modifiers.Length; i++)
            {
                _stat.RemoveModifier(_modifiers[i]);
            }
        }


        // one passive mod skill is that every 3rd melee attack will deal physical and elemental damage.






    }
}