using UnityEngine;

namespace Game
{
    public class NPCDamageProcessor : MonoBehaviour
    {
        [SerializeField] private NPCHealthHandler _healthHandler;
        [SerializeField] private Transform _forward;

        private float FacingDirection => Mathf.Sign(_forward.position.x - transform.position.x);

        public CombatCollisionResult ProcessDamage(Damage damage, HandleDamageBehaviour behaviourSettings)
        {
            if (damage == null)
            {
                Logger.Error("Null Damage!");
                return CombatCollisionResult.Ignored;
            }

            // check if blocking is possible.
            else if (WasAttackSuccessfullyBlocked(damage, behaviourSettings.BlockingConfig)) return OnSuccessfulBlockResult(behaviourSettings.BlockingConfig);

            // if logic gets here, then damage can be taken.
            CalculateDamageAndApplyHealthChanges(damage);

            // return different results depending on health data and behaviour settings.
            return OnTakeDamageResult(damage, behaviourSettings);
        }

        private void CalculateDamageAndApplyHealthChanges(Damage damage)
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

                float dmgTaken = 0;

                switch (type)
                {
                    case DamageType.Physical:
                        dmgTaken = Damage.GetDamageDealt(raw, ign, _healthHandler.PhysicalResist);
                        damageTypes[i] = new HealthDamage(dmgTaken, DamageType.Physical);
                        break;
                    case DamageType.Electrical:
                        dmgTaken = Damage.GetDamageDealt(raw, ign, _healthHandler.ThermalResist);
                        damageTypes[i] = new HealthDamage(dmgTaken, DamageType.Electrical);
                        break;
                    case DamageType.Thermal:
                        dmgTaken = Damage.GetDamageDealt(raw, ign, _healthHandler.ElectricalResist);
                        damageTypes[i] = new HealthDamage(dmgTaken, DamageType.Thermal);
                        break;
                    case DamageType.Chemical:
                        dmgTaken = Damage.GetDamageDealt(raw, ign, _healthHandler.ChemicalResist);
                        damageTypes[i] = new HealthDamage(dmgTaken, DamageType.Chemical);
                        break;
                    default: break;
                }

                totalDamageTaken += dmgTaken;
            }

            damageTypesTaken.TotalDamage = totalDamageTaken;
            damageTypesTaken.DamageTypes = damageTypes;

            _healthHandler.AddHealth(-totalDamageTaken);
            _healthHandler.AddStaggerDamage(damage.StaggerDamage);
        }

        private bool WasAttackSuccessfullyBlocked(Damage damage, DamageBlockingConfig blockConfig)
        {
            var blockMode = blockConfig.Mode;
            bool blocked = false;

            if (!blockConfig.IsActive) blocked = false;
            else if (blockMode == BlockMode.FullCircle) blocked = true;
            else if (blockMode == BlockMode.Directional)
            {
                //check if enemy facing direction == direction from enemy to attack source.
                float attackDirection = Mathf.Sign(damage.AttackSource.transform.position.x - transform.position.x);
                blocked = (attackDirection == FacingDirection);
            }

            if (blocked)
            {
                // increment block config's block count by 1.
            }

            return blocked;
        }

        private CombatCollisionResult OnSuccessfulBlockResult(DamageBlockingConfig blockConfig)
        {
            // check if block config's block count exceeds block threshold.
            // if so, return block broken. otherwise, return block.

            return CombatCollisionResult.Block;
        }

        private CombatCollisionResult OnTakeDamageResult(Damage damage, HandleDamageBehaviour settings)
        {
            var hurtConfig = settings.HurtConfig;

            // death: when health is nonpositive.
            if (_healthHandler.CurrentHealth <= 0) return CombatCollisionResult.Death;

            // heavy knockback: if allowed in settings and if can stagger && can knockback && attack is strong enough.
            else if (hurtConfig.CanHeavyKnockback
                  && damage.AttackStrength > _healthHandler.AttackStrengthResistance
                  && _healthHandler.CanStagger
                  && _healthHandler.KnockbackReady)
                return CombatCollisionResult.HeavyKnockback;

            else if (hurtConfig.CanLightKnockback
                  && damage.AttackStrength >= _healthHandler.AttackStrengthResistance
                  && _healthHandler.KnockbackReady)
                return CombatCollisionResult.LightKnockback;

            else if (hurtConfig.CanStagger && _healthHandler.CanStagger) return CombatCollisionResult.Stagger;

            else return CombatCollisionResult.Normal;
        }
    }

    public enum CombatCollisionResult
    {
        Normal,
        Stagger,
        LightKnockback,
        HeavyKnockback,
        Block,
        BrokenBlock,
        Death,
        Overheated, // elemental
        Shocked, // elemental
        Corroded, // elemental
        Ignored
    }
}