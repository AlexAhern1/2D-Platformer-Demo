using UnityEngine;

namespace Game.Player
{
    public class PlayerFXMediator : MonoBehaviour
    {
        [Header("FX Config")]
        [SerializeField] private FXPlayer _fxs;
        [SerializeField] private int _onHitFx;

        [Header("SO Events")]
        [SerializeField] private PlayerTakeDamageEvent _takeDamageEvent;

        [Header("SO Resources")]
        [SerializeField] private ResourceSO _healthResource;

        // create another script that will handle state transitions based on damage taken.

        private void OnEnable()
        {
            _takeDamageEvent.AddEvent(OnTakeDamage);
        }

        private void OnDisable()
        {
            _takeDamageEvent.RemoveEvent(OnTakeDamage);
        }

        private void OnTakeDamage(PlayerHealthChangeData data)
        {
            if (_healthResource.Current == 0)
            {
                // death fx
                Logger.Log("TODO - play death FX.");
            }
            else
            {
                _fxs.Play(_onHitFx);
            }
        }
    }
}