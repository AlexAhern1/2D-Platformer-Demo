using UnityEngine;

namespace Game
{
    public class NPCStateMachine : MonoBehaviour
    {
        [Header("State Config")]
        [SerializeField] private int _defaultStateIndex;
        [SerializeField] private NPCState[] _states;

        [Header("Transition Handler")]
        [SerializeField] private NPCStateTransitionHandler _transitionHandler;

        private NPCState _currentState;

        public void Initialize()
        {
            _transitionHandler.Initialize();

            _currentState = _states[_defaultStateIndex];
        }

        public void Enable()
        {
            _currentState.Enter();
        }

        public void Disable()
        {
            _currentState.Exit();
        }

        public void Run()
        {
            while (_transitionHandler.PendingTransition)
            {
                int nextID = _transitionHandler.GetNextTransitionID();

                // check if the ID is even valid.
                if (nextID >= _states.Length)
                {
                    Logger.Error($"Invalid transition ID - {nextID}, when there are only {_states.Length} states for {this}", MoreColors.BrightRed);
                    
                }
                else
                {
                    // exit current state
                    _currentState.Exit();

                    // update current state
                    _currentState = _states[nextID];

                    // enter current state
                    _currentState.Enter();
                }
            }
        }

        public void FixedRun()
        {
            // update current state.
            _currentState.FixedRun();
        }


        private void Awake()
        {
            // called in awake because OnValidate doesn't get called in a build.
            SetStateIDs();
        }

        private void OnValidate()
        {
            SetStateIDs();
        }

        private void SetStateIDs()
        {
            for (int i = 0; i < _states.Length; i++)
            {
                if (_states[i] == null) continue;
                _states[i].SetID(i);
            }
        }
    }
}