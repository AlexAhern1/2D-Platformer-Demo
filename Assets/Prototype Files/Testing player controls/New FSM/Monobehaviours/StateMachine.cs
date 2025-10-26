using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class StateMachine : MonoBehaviour, IInitializable, IEnable
    {
        [SerializeField] private int _initialQueueSize;
        [SerializeField] private PlayerState _currentState;

        private Queue<PlayerState> _transitionQueue;

        //failsafes
        private int c;

        // properties
        private bool PendingTransition => _transitionQueue.Count > 0;

        public void Initialize()
        {
            _transitionQueue = new(_initialQueueSize);
        }

        public void Enable()
        {
            _currentState.Enter();
        }

        public void Disable()
        {
            _currentState.Exit();
        }

        public void DoTransition(PlayerState state)
        {
            _transitionQueue.Enqueue(state);
        }

        private void Update()
        {
            while (PendingTransition && c < 100)
            {
                c++;

                _currentState.Exit();
                _currentState = _transitionQueue.Dequeue();
                _currentState.Enter();

                if (!PendingTransition) c = 0;
            }

            if (c == 100)
            {
                Logger.Error("INFINITE LOOP");
                _transitionQueue.Clear();
                c = 0;
            }
        }
    }
}