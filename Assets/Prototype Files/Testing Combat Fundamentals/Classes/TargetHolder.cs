using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class TargetHolder
    {
        public string Name;
        [SerializeField] private bool _inspectorEditable;
        [SerializeField] private GameObject _target;

        public GameObject Target => _target;

        public void SetTarget(GameObject target) => _target = target;
    }
}