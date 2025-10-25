using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CameraChangeSettings
    {
        [SerializeReference, SubclassSelector]
        public ICameraController CameraController;
        public float CameraChangeDuration;
    }
}