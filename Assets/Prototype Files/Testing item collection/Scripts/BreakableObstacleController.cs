using UnityEngine;

namespace Game
{
    public class BreakableObstacleController : MonoBehaviour
    {
        [SerializeField] private BreakableObject _breakable;

        private void OnEnable()
        {
            _breakable.BreakEvent += OnBreak;
        }

        private void OnDisable()
        {
            _breakable.BreakEvent -= OnBreak;
        }

        private void OnBreak(Damage dmg)
        {
            _breakable.gameObject.SetActive(false);
        }
    }
}