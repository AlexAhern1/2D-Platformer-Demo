using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class FollowObjectCamera : ICameraController
    {
        public float Size;
        public float Angle;

        [Header("In the future, use an ITransformGetter")]
        public Transform FollowTarget;

        public CameraData UpdateCamera()
        {
            Vector2 pos = FollowTarget.position;
            return new CameraData(pos.x, pos.y, Angle, Size);
        } 
    }
}