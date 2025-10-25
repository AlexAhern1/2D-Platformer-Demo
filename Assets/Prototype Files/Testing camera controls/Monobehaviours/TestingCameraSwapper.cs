using System;
using UnityEngine;

namespace Game
{
    [Obsolete]
    public class TestingCameraSwapper : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        public ICameraController _strategy;
        [SerializeField] private float _switchTime;

        [Header("easier way to handle bounds")]
        [SerializeField] private Vector2 _center;
        [SerializeField] private Vector2 _size;

        [SerializeReference, SubclassSelector]
        public ICameraShaker _shaker;
        [SerializeField] private float _shakeDuration;
        [SerializeField] private CameraController _controller;

        [Header("Settings")]
        [SerializeField] private bool _applyControllerOnStart;

        //public void Swap()
        //{
        //    Logger.Log("swapping cameras");
        //    Rect bounds = new Rect(_center - 0.5f * _size, _size);

        //    //_controller.SwitchCamera(_strategy, bounds, _switchTime);
        //}

        //public void Shake()
        //{
        //    _controller.SetCameraShake(_shaker, _shakeDuration);
        //}

        //private void Start()
        //{
        //    if (_applyControllerOnStart) Swap();
        //}

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(_center, _size);
        }
    }
}