using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class ColliderDebugger : MonoBehaviour
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private float _drawDuration = 5f;

        [SerializeField] private InputSystemDistributor _distributor;

        [SerializeField] private LayerMask _layerMask;

        private Collider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            _distributor.InputSystem.Level.Debug.started += OnDebugKeyPressed;
        }

        private void OnDisable()
        {
            _distributor.InputSystem.Level.Debug.started -= OnDebugKeyPressed;
        }

        private void OnDebugKeyPressed(InputAction.CallbackContext context)
        {
            if (!_enabled) return;
            ShowCollisionBounds();
        }

        private void ShowCollisionBounds()
        {
            Logger.DrawBox(_collider.bounds, _drawDuration, Color.green);

            RaycastHit2D[] hits = Physics2D.BoxCastAll(_collider.bounds.center, _collider.bounds.size, 0f, Vector2.zero, 0f, _layerMask);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider == null) continue;
                Logger.DrawBox(hit.collider.bounds, _drawDuration, Color.yellow);
            }
        }
    }
}