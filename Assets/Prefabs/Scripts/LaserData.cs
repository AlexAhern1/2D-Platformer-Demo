using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class LaserData
    {
        //damage parameters
        //public DamageDataOLD InitialDamage;
        //public DamageDataOLD TickDamage;
        public float TickFrequency;

        //world parameters
        public Vector3 FocusPoint;
        public Quaternion Rotation;
        public float Length;
    }
}