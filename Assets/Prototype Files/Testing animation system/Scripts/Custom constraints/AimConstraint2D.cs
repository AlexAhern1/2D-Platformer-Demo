using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

namespace Game
{
    [DisallowMultipleComponent, AddComponentMenu("Animation Rigging/2D/Aim Constraint")]
    public class AimConstraint2D : RigConstraint<AimConstraint2DJob,
                                                 AimConstraint2DData,
                                                 AimConstraint2DJobBinder> { }

    public struct AimConstraint2DJob : IWeightedAnimationJob
    {
        public ReadWriteTransformHandle Constrained;

        public ReadOnlyTransformHandle Center;
        public ReadOnlyTransformHandle Source;

        public FloatProperty MaxRadius;

        public FloatProperty jobWeight { get; set; }

        
        public void ProcessAnimation(AnimationStream stream)
        {
            float w = jobWeight.Get(stream);
            if (w > 0f) SetConstrainedToFaceSource(stream, w);
        }

        // this is always empty.
        public void ProcessRootMotion(AnimationStream stream) { }

        private void SetConstrainedToFaceSource(AnimationStream stream, float w)
        {
            Vector2 centerPosition = Center.GetPosition(stream);

            Vector2 fullDirection = (Vector2)Source.GetPosition(stream) - centerPosition;
            float r = Mathf.Min(fullDirection.magnitude, MaxRadius.Get(stream)) * w;

            Constrained.SetPosition(stream, centerPosition + r * fullDirection.normalized);
        }
    }

    [Serializable]
    public struct AimConstraint2DData : IAnimationJobData
    {
        public Transform ConstrainedObject;
        public Transform CenterObject;
        [SyncSceneToStream] public float MaxRadius;

        [SyncSceneToStream] public Transform SourceObject;

        public bool IsValid() => !(ConstrainedObject == null || SourceObject == null);

        public void SetDefaultValues()
        {
            ConstrainedObject = null;
            SourceObject = null;
        }
    }

    public class AimConstraint2DJobBinder : AnimationJobBinder<AimConstraint2DJob, AimConstraint2DData>
    {
        public override AimConstraint2DJob Create(Animator animator, ref AimConstraint2DData data, Component component)
        {
            return new AimConstraint2DJob()
            {
                Constrained = ReadWriteTransformHandle.Bind(animator, data.ConstrainedObject),

                Source = ReadOnlyTransformHandle.Bind(animator, data.SourceObject),
                Center = ReadOnlyTransformHandle.Bind(animator, data.CenterObject),


                MaxRadius = FloatProperty.Bind(animator, component, ConstraintsUtils.ConstructConstraintDataPropertyName(nameof(data.MaxRadius)))
            };
        }

        public override void Destroy(AimConstraint2DJob job) { }
    }
}