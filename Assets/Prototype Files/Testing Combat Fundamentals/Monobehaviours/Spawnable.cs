using System;
using UnityEngine;

namespace Game
{
    public abstract class Spawnable : MonoBehaviour, ISpawnable
    {
        public abstract void Spawn();

        public abstract void Despawn();
    }
}