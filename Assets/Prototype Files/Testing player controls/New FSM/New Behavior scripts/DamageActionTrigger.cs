using System;
using UnityEngine;

namespace Game.Player
{
    [Serializable]
    public class DamageActionTrigger : ActionTrigger
    {
        [Separator]
        [Header("Damage Data")]
        public PlayerTakeDamageEvent DamageEvent;
        public ResourceSO Health;

        [Separator(2)]
        [Header("Damage Conditions")]
        [Separator]
        public bool CompareCurrentHealth;
        [Separator]
        [SerializeReference, SubclassSelector]
        public IFloatGetter CompareValue;
        public ComparisonOperator Comparer;
        [Separator]
        [Header("Weapon Type")]
        [Separator]
        public bool CheckWeaponType;
        [Separator]
        public WeaponType Type;
        public bool Match;

        public override void Start(Action callback)
        {
            this.callback = callback;
            DamageEvent.AddEvent(OnTakeDamage);
        }

        public override void Stop(Action callback)
        {
            this.callback = null;
            DamageEvent.RemoveEvent(OnTakeDamage);
        }

        private void OnTakeDamage(PlayerHealthChangeData data)
        {
            bool result = true;

            if (CompareCurrentHealth)
            {
                result &= Comparer.Evaluate(Health.Current, CompareValue.GetFloat());
            }

            if (CheckWeaponType)
            {
                result &= MyExtensions.Iff(Match, data.Weapon == Type);
            }

            if (result) callback.Invoke();
        }
    }
}