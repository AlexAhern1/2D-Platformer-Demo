using UnityEngine;

namespace Game
{
    public class SceneBorder : MonoBehaviour
    {
        [Header("Transition Strategy")]
        [SerializeReference, SubclassSelector]
        private ISceneTransitionStrategy _transitionStrategy;

        [Separator]

        [Header("Scene config - A")]
        // these two can be wrapped in a class (scene network) if this works.
        [SerializeField] private SceneField _targetCentralSceneA;
        [SerializeField] private SceneField[] _targetAdjacentScenesA;
        [SerializeField] private Transform _exitPointA;

        [Separator]

        [SerializeField] private SceneField _targetCentralSceneB;
        [SerializeField] private SceneField[] _targetAdjacentScenesB;
        [SerializeField] private Transform _exitPointB;

        [Header("Collision config")]
        [SerializeField] private Tag _playerTag;

        [Header("SO Events")]
        [SerializeField] private SceneTransitionRequestEvent _requestEvent;

        [Header("Debugging")]
        [SerializeField] private bool _showVoronoi;
        [SerializeField] private float _borderLength;
        [SerializeField] private Color _borderColor;

        //requests
        private SceneTransitionRequest _requestA;
        private SceneTransitionRequest _requestB;

        private void Awake()
        {
            _requestA = new(_targetCentralSceneA, _targetAdjacentScenesA, _transitionStrategy);
            _requestB = new(_targetCentralSceneB, _targetAdjacentScenesB, _transitionStrategy);
        }

        private void OnPlayerExitTrigger(Vector2 playerPosition)
        {
            //get the square distance between the player position and the two exit points

            float exitA_SquareDistance = GetSquareDistance(_exitPointA.position, playerPosition);
            float exitB_SquareDistance = GetSquareDistance(_exitPointB.position, playerPosition);

            if (exitA_SquareDistance < exitB_SquareDistance)
            {
                _requestEvent.Raise(_requestA);
            }
            else if (exitA_SquareDistance > exitB_SquareDistance)
            {
                _requestEvent.Raise(_requestB);
            }
            else
            {
                Logger.Error("Scene Loader found equal distance!", Logger.SceneLoading, MoreColors.Scarlet);
            }
        }

        private float GetSquareDistance(Vector2 a, Vector2 b)
        {
            return Vector2.SqrMagnitude(b - a);
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (!collider.CompareTag(_playerTag)) return;
            OnPlayerExitTrigger(collider.transform.position);
        }

        private void OnDrawGizmos()
        {
            if (!_showVoronoi) return;

            // get midpoint of exit points
            Vector2 midpoint = 0.5f * (_exitPointA.position + _exitPointB.position);

            // get normal vector
            Vector2 direction = (_exitPointA.position - _exitPointB.position).normalized;

            Vector2 halfBorder = 0.5f * _borderLength * direction.Normal();

            Gizmos.color = _borderColor;
            Gizmos.DrawLine(midpoint - halfBorder, midpoint + halfBorder);
        }
    }
}