using System.Collections.Generic;
using UnityEngine;

namespace Game.World
{
    public abstract class WaypointHandler : MonoBehaviour
    {
        [SerializeField] protected ObjectMover objectMover;
        [SerializeField] protected List<Transform> waypoints;

        [SerializeField] protected float objectSpeed;

        protected int currentIndex;

        protected Transform moverTransform;
        protected Vector3 currentTarget;

        float xSign;
        float ySign;

        void Start()
        {
            moverTransform = objectMover.transform;

            if (waypoints.Count < 2)
            {
                Debug.LogWarning("too few waypoints!");
                return;
            }
            currentIndex = 1;
            SetWaypoint(currentIndex);
        }

        //TODO - implement an oclusion culling algorithm for this script to execute to predict platform position. Update will be kept for smooth animations
        void Update()
        {
            if (SignsFlipped(currentTarget, moverTransform.position))
            {
                moverTransform.position = currentTarget;
                ReachTarget();
            }
        }

        protected virtual void SetWaypoint(int index)
        {
            currentTarget = waypoints[index].position;

            xSign = Mathf.Sign(currentTarget.x - moverTransform.position.x);
            ySign = Mathf.Sign(currentTarget.y - moverTransform.position.y);
        }

        protected abstract void ReachTarget();

        protected void MoveToNextWaypoint()
        {
            objectMover.SetTargetSpeed(currentTarget, objectSpeed);
        }

        bool SignsFlipped(Vector3 current, Vector3 target)
        {
            if (target.y == current.y)
            {
                return Mathf.Sign(current.x - target.x) != xSign;
            }
            else if (target.x == current.x)
            {
                return Mathf.Sign(current.y - target.y) != ySign;
            }
            bool xFlipped = Mathf.Sign(current.x - target.x) != xSign;
            bool yFlipped = Mathf.Sign(current.y - target.y) != ySign;
            return xFlipped && yFlipped;
        }

        private void OnDrawGizmosSelected()
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                Gizmos.color = Color.white;
                if (i == waypoints.Count - 1)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                }
                else
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i+1].position);
                }
            }
        }
    }
}