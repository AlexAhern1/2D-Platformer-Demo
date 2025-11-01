using System;
using UnityEngine;

namespace Game
{
    public enum DamageType { Physical, Thermal, Electrical, Chemical }

    public enum WeaponType { Melee, Range, Hazard, Field }

    [Serializable]
    public class Damage
    {
        public HealthDamageWithIgnorance[] RawDamage;

        public WeaponType Weapon;
        public int AttackStrength;
        public float StaggerDamage;

        public GameObject AttackSource;
        public GameObject Attacker;

        public float TotalDamage()
        {
            float dmg = 0;
            for (int i = 0; i < RawDamage.Length; i++)
            {
                dmg += RawDamage[i].DamageType.Amount;
            }
            return dmg;
        }

        public static float GetDamageDealt(float amount, float ignoreResist, float resist)
        {
            resist = Mathf.Max(0.01f, resist); //ensures no division by 0.
            return amount * (1.5f + (ignoreResist - resist) / resist);
        }
    }

    [Serializable]
    public struct HealthDamage
    {
        public HealthDamage(float amount, DamageType type)
        {
            Amount = amount;
            Type = type;
        }

        public float Amount;
        public DamageType Type;
    }

    [Serializable]
    public struct HealthDamageWithIgnorance
    {
        public HealthDamageWithIgnorance(float amount, float ignoreResist, DamageType type)
        {
            DamageType = new HealthDamage(amount, type);
            IgnoreResistance = ignoreResist;
        }

        public HealthDamage DamageType;
        public float IgnoreResistance;
    }

    public readonly struct DamageDealt
    {
        public DamageDealt(float amount, DamageType type)
        {
            Amount = amount;
            Type = type;
        }

        public readonly float Amount { get; }
        public readonly DamageType Type { get; }
    }
}