using UnityEngine;

namespace Game.Player
{
    public class PlayerDamageable : Damageable
    {
        [SerializeField] private Damage _selfDamage;
        [SerializeField] private PlayerFSMBlackboard _blackboard;

        [Header("Resources")]
        [SerializeField] private ResourceSO _playerHealthResource;

        // NOTE - observing max health changes (and updating current health) should be done in another script.
        // for now, do everything here as a proper FLOW of logic has yet to be defined.
        [SerializeField] private Stat _maxHealthStat;

        [Header("Events")]
        [SerializeField] private PlayerTakeDamageEvent _takeDamageEvent;

        [Header("Resistances (stored here until shared)")]
        [SerializeField][Range(0.01f, 100f)] private float _physicalResist;
        [SerializeField][Range(0.01f, 100f)] private float _thermalResist;
        [SerializeField][Range(0.01f, 100f)] private float _electricalResist;
        [SerializeField][Range(0.01f, 100f)] private float _chemicalResist;
        [SerializeField] private float _staggerResistance;

        public override void TakeDamage(Damage damage)
        {
            // initialize damage taken struct and its damage types
            DamageTaken damageTypesTaken = new DamageTaken();
            HealthDamage[] damageTypes = new HealthDamage[damage.RawDamage.Length];

            float totalDamageTaken = 0;

            // loop through each raw damage type and calculate total damage with resistance.
            for (int i = 0; i < damage.RawDamage.Length; i++)
            {
                var dmg = damage.RawDamage[i];
                DamageType type = dmg.DamageType.Type;

                float raw = dmg.DamageType.Amount;
                float ign = dmg.IgnoreResistance;

                float dmgTaken = 0f;
                damageTypes[i] = new HealthDamage(dmgTaken, type);

                switch (type)
                {
                    case DamageType.Physical:
                        dmgTaken = Damage.GetDamageDealt(raw, ign, _physicalResist);
                        damageTypes[i] = new HealthDamage(dmgTaken, DamageType.Physical);
                        break;
                    case DamageType.Thermal:
                        dmgTaken = Damage.GetDamageDealt(raw, ign, _thermalResist);
                        damageTypes[i] = new HealthDamage(dmgTaken, DamageType.Thermal);
                        break;
                    case DamageType.Electrical:
                        dmgTaken = Damage.GetDamageDealt(raw, ign, _electricalResist);
                        damageTypes[i] = new HealthDamage(dmgTaken, DamageType.Electrical);
                        break;
                    case DamageType.Chemical:
                        dmgTaken = Damage.GetDamageDealt(raw, ign, _chemicalResist);
                        damageTypes[i] = new HealthDamage(dmgTaken, DamageType.Chemical);
                        break;
                }

                totalDamageTaken += dmgTaken;
            }

            damageTypesTaken.TotalDamage = totalDamageTaken;
            damageTypesTaken.DamageTypes = damageTypes;

            _playerHealthResource.Add(-totalDamageTaken);

            RaiseDamageEvent(damage);

            var healthChangeData = new PlayerHealthChangeData()
            {
                DamageTaken = damageTypesTaken,
                Weapon = damage.Weapon,
                StaggerStrength = damage.StaggerDamage,
                StaggerResistance = _staggerResistance
            };

            _takeDamageEvent.Raise(healthChangeData);
        }

        public void SelfInflictDamage()
        {
            Logger.Log("Self-inflicted Damage.", Logger.Combat, MoreColors.Lilac);
            TakeDamage(_selfDamage);
        }

        private void OnEnable()
        {
            _maxHealthStat.AddOnChangeEvent(OnMaxHealthChange);
        }

        private void OnDisable()
        {
            _maxHealthStat.RemoveOnChangeEvent(OnMaxHealthChange);
        }

        private void OnMaxHealthChange(float newMaxHealth)
        {
            _playerHealthResource.Set(Mathf.Min(_playerHealthResource.Current, newMaxHealth));
        }
    }
}