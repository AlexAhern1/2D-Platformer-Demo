using UnityEngine;

namespace Game.Player
{
    public class PlayerLedgeGrabDetector : MonoBehaviour, IStateAction, IToggle
    {
        [Header("Components")]
        [SerializeField] private CollisionHandler _collisions;

        [Header("Layer mask")]
        [SerializeField] private LayerMask _obstacleLayer;

        [Header("obstacle collider")]
        [SerializeField] private Vector2 _obstacleColliderCenter;
        [SerializeField] private Vector2 _obstacleColliderSize;

        [Header("space collider")]
        [SerializeField] private Vector2 _spaceColliderCenter;
        [SerializeField] private Vector2 _spaceColliderSize;

        [Header("Direction getter")]
        [SerializeReference, SubclassSelector] private IFloatGetter _directionGetter;

        [Header("Placement")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _xOffset;
        [SerializeField] private float _yOffset;

        [Header("events")]
        [SerializeField] private GameEvent _ledgeGrabEvent;

        // state variables
        private readonly Collider2D[] _colliders = new Collider2D[1];
        private bool _obstacleColliderFound;
        private bool _spaceColliderEmpty;

        private Vector2 center => transform.position;

        public void DoAction()
        {
            // for now, just set the player's transform.position to the target position.
            //_playerTransform.position = _targetTransform.position;

            // check if the collider is null (ideally, it should NEVER be null)
            var obstacle = _colliders[0];
            if (obstacle == null)
            {
                Logger.Error("Ledge grab detector found a null obstacle!");
                return;
            }

            // get the component that calculates the bounding box of the collider.
            // currently, a terrain prefab uses the sprite renderer to control its size, so the transform component
            // will always give a scale of 1, which will be incorrect.

            // need a component: IBound, which returns a rect with the position and size.
            // in the future, maybe once greyboxes are no longer needed, we might need to change the way we obtain bounding boxes.
            // so, an interface may serve this need well.

            var obstacleRectGetter = obstacle.GetComponent<IRectGetter>();
            if (obstacleRectGetter == null)
            {
                Logger.Error("Ledge grab detector found an obstacle with no rect getter!");
                return;
            }

            Rect obstacleRect = obstacleRectGetter.GetRect();

            // need to place the player:
            //  X1 to the LEFT/RIGHT of X2
            //  Y1 above Y2

            // 5 variables needed:
            //  X1 = distance offset to move the player forward
            //  X2 = x-corner that's closer to the player
            //  LEFT/RIGHT: obtained from facing direction (ifloatgetter)
            //  Y1 = height offset to move the player up.
            //  Y2 = max y-value.

            float directionToWall = _directionGetter.GetFloat();

            float xCorner = obstacleRect.position.x - directionToWall * obstacleRect.size.x * 0.5f;
            float yTop = obstacleRect.position.y + obstacleRect.size.y * 0.5f;

            Vector2 teleportPosition = new(xCorner + directionToWall * _xOffset, yTop + _yOffset);

            //manually remove the collision towards the obstacle first.
            _collisions.RemoveCollision(obstacle);

            _playerTransform.position = teleportPosition;

            //reintroduce the same collision but downwards this time.
            _collisions.AddManualCollision(obstacle, Vector2.down);
        }

        public void Toggle(bool on)
        {
            gameObject.SetActive(on);
        }

        private void FixedUpdate()
        {
            float dir = _directionGetter.GetFloat();

            _obstacleColliderCenter.x = dir * Mathf.Abs(_obstacleColliderCenter.x);
            _spaceColliderCenter.x = dir * Mathf.Abs(_spaceColliderCenter.x);

            _spaceColliderEmpty = Physics2D.OverlapBoxNonAlloc(center + _spaceColliderCenter, _spaceColliderSize, 0, _colliders, _obstacleLayer) == 0;
            _obstacleColliderFound = Physics2D.OverlapBoxNonAlloc(center + _obstacleColliderCenter, _obstacleColliderSize, 0, _colliders, _obstacleLayer) > 0;

            if (_obstacleColliderFound && _spaceColliderEmpty) _ledgeGrabEvent.Raise();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = _obstacleColliderFound ? Color.green : Color.red;
            Gizmos.DrawWireCube(center + _obstacleColliderCenter, _obstacleColliderSize);

            Gizmos.color = _spaceColliderEmpty ? Color.green : Color.red;
            Gizmos.DrawWireCube(center + _spaceColliderCenter, _spaceColliderSize);
        }
    }
}