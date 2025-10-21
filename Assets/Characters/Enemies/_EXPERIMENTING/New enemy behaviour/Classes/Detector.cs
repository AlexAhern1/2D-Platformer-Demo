using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public abstract class Detector
    {
        public string InspectorName;
        public LayerMask DetectLayer;
        public Vector2 Offset;

        [Header("For debugging")]
        public Color GizmosColor;
        public bool HideInInspector;

        protected Collider2D[] detectionArray = new Collider2D[1];

        public GameObject Detect(Vector2 center, float facingDirection)
        {
            detectionArray[0] = null;
            Vector2 detectionPoint = center + new Vector2(facingDirection * Offset.x, Offset.y);
            bool entityFound = EntityFound(detectionPoint);
            DrawDetectorShape(detectionPoint, ColorValue(entityFound));

            return detectionArray[0] == null ? null : detectionArray[0].gameObject;
        }

        public virtual void DrawGizmos(Vector2 center, float facingDirection) { }

        protected abstract bool EntityFound(Vector2 detectionCenter);

        protected abstract void DrawDetectorShape(Vector2 center, Color color);

        protected Color ColorValue(bool found) { return found ? Color.green : Color.red; }
    }

    [Serializable]
    public class PointDetector : Detector
    {
        public override void DrawGizmos(Vector2 center, float facingDirection)
        {
            Gizmos.color = GizmosColor;
            Gizmos.DrawWireSphere(center + new Vector2(facingDirection * Offset.x, Offset.y), 0.25f);
        }

        protected override bool EntityFound(Vector2 detectionPoint)
        {
            return Physics2D.OverlapPointNonAlloc(detectionPoint, detectionArray, DetectLayer) == 1;
        }

        protected override void DrawDetectorShape(Vector2 detectionPoint, Color color)
        {
            Logger.DrawCircle(detectionPoint, 0.1f, 0.5f, color);
        }
    }

    [Serializable]
    public class CircleDetector : Detector
    {
        public float Radius;

        public override void DrawGizmos(Vector2 center, float facingDirection)
        {
            Gizmos.color = GizmosColor;
            Gizmos.DrawWireSphere(center + new Vector2(facingDirection * Offset.x, Offset.y), Radius);
        }

        protected override bool EntityFound(Vector2 detectionPoint)
        {
            return Physics2D.OverlapCircleNonAlloc(detectionPoint, Radius, detectionArray, DetectLayer) == 1;
        }

        protected override void DrawDetectorShape(Vector2 detectionPoint, Color color)
        {
            Logger.DrawCircle(detectionPoint, Radius, 0.1f, color);
        }
    }

    [Serializable]
    public class RectDetector : Detector
    {
        public Vector2 Size;

        public override void DrawGizmos(Vector2 center, float facingDirection)
        {
            Gizmos.color = GizmosColor;
            Gizmos.DrawWireCube(center + new Vector2(facingDirection * Offset.x, Offset.y), 2f * Size);
        }

        protected override bool EntityFound(Vector2 detectionPoint)
        {
            return Physics2D.OverlapBoxNonAlloc(detectionPoint, Size * 2f, 0f, detectionArray, DetectLayer) == 1;
        }

        protected override void DrawDetectorShape(Vector2 detectionPoint, Color color)
        {
            Logger.DrawBox(detectionPoint, Size, 0.1f, color);
        }
    }
}