using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class NPCStateTransitionHandler : MonoBehaviour
    {
        [SerializeField] private int _initialQueueSize;
        private bool _pendingTransition;

        private Queue<int> _transitionIDQueue;

        public bool PendingTransition => _pendingTransition;

        public void Initialize()
        {
            _transitionIDQueue = new Queue<int>(_initialQueueSize);
        }

        public void DoTransition(int id)
        {
            if (id < 0) return;

            _transitionIDQueue.Enqueue(id);
            _pendingTransition = true;
        }

        public void DoTransition(NPCState nextState)
        {
            Logger.Log($"Transition to {nextState.ID}", Logger.EnemyCombat);

            DoTransition(nextState.ID);
        }

        public int GetNextTransitionID()
        {
            int nextID = _transitionIDQueue.Dequeue();
            _pendingTransition = _transitionIDQueue.Count > 0;
            return nextID;
        }
    }
}