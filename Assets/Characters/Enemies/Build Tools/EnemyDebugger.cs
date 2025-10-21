using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Enemy
{
    public class EnemyDebugger : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private NPCState _state;

        public void OnPointerClick(PointerEventData eventData)
        {
            _state.DebugBehaviourActivity();
        }
    }
}