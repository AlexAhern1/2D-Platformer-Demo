using System.Collections.Generic;
using UnityEngine;

namespace Game.World
{
    [RequireComponent(typeof(Collider2D))]
    public class DoorTrap : MonoBehaviour
    {
        [SerializeField] private Tag _playerTag;

        [SerializeField] private List<Door> _doors;

        protected bool trapActivated;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag(_playerTag) || trapActivated) return;
            trapActivated = true;

            OnPlayerTriggerTrap();
        }

        protected virtual void OnPlayerTriggerTrap()
        {
            LockDoors();
        }

        protected void LockDoors()
        {
            for (int i = 0; i < _doors.Count; i++)
            {
                Door door = _doors[i];
                door.Close();
            }
        }

        protected void UnlockDoors()
        {
            for (int i = 0; i < _doors.Count; i++)
            {
                Door door = _doors[i];
                door.Open();
            }
        }
    }
}