using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Base class for the controlled usage of any kind of attack.
    /// </summary>
    public abstract class AttackController : MonoBehaviour
    {
        [Tooltip("Optional")]
        [SerializeField] private string _attackName;

        //events
        protected event Action<GameObject> hitTargetEvent;

        public void AddEvent(Action<GameObject> e) => hitTargetEvent += e;

        public void RemoveEvent(Action<GameObject> e) => hitTargetEvent -= e;

        public abstract void UseAttack();

        public virtual void StopAttack() { }

        protected void RaiseHitTargetEvent(GameObject target) => hitTargetEvent?.Invoke(target);
    }
}