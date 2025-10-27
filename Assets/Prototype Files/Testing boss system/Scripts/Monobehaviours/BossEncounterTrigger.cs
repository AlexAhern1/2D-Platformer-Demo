using System;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public class BossEncounterTrigger : MonoBehaviour, IGameEvent<GameObject>
    {
        [SerializeField] private bool _isFirstEncounter;
        [SerializeField] private Tag _playerTag;
        [SerializeField] private BossDataSO _data;

        public event Action<GameObject> GameEvent;

        public void AddEvent(Action<GameObject> e) => GameEvent += e;

        public void RemoveEvent(Action<GameObject> e) => GameEvent -= e;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(_playerTag)) return;
            GameEvent?.Invoke(collision.gameObject);
        }

        // what must happen?
        //  1) enter trigger area
        //  2) block exits
        //  3) enable boss health bar in HUD
        //  4) swap camera
        //  5) listen for when the player dies OR when the boss dies. (different consequences for each)
    }
}