using UnityEngine;

namespace Game.UI
{
    public class UIPanelGraph : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private IVector2Getter _directionGetter;

        // there will be a default selected button when opening the panel for the first time.
        [Separator]
        [Header("Graph config")]
        [SerializeField] private SerializedDictionary<UIObject, UITarget[]> _targetObjects;
        [Separator]

        [Header("Initial config")]
        [SerializeField] private UIObject _currentObject;
        [SerializeField] private bool _initializeOnAwake;

        [Header("Math config")]
        [SerializeField] private float _angleTolerance;

        private void Awake()
        {
            // verify that the default object is part of the targets dictionary.
            if (!_targetObjects.ContainsKey(_currentObject))
            {
                Logger.Error($"{_currentObject} is not present in the targets dictionary");
                return;
            }

            else if (_initializeOnAwake)
            {
                _currentObject.Initialize();
            }
        }

        public void Initialize()
        {
            _currentObject.Initialize();
        }

        public void UpdateObjects()
        {
            var value = _directionGetter.Get().normalized;
            if (value == Vector2.zero) return;

            var potentialTargets = _targetObjects[_currentObject];

            float minAngle = Mathf.PI;
            UIObject nearestObject = null;

            for (int i = 0; i < potentialTargets.Length; i++)
            {
                var target = potentialTargets[i];
                var dir = target.Direction.normalized;

                float angle = Mathf.Acos(Vector2.Dot(value, dir));

                if (Mathf.Abs(angle) < Mathf.Min(minAngle, _angleTolerance * Mathf.Deg2Rad))
                {
                    nearestObject = target.Target;
                    minAngle = angle;
                }
            }

            if (nearestObject == null) return;

            _currentObject.Exit();
            _currentObject = nearestObject;
            _currentObject.Enter();
        }

        public void SelectCurrentObject()
        {
            _currentObject.Select();
        }
    }

    // first functionality -> button presses should be able to close a current panel and open up another panel.
    // then, the UI PANEL CONTROL SCHEME should change to allow controls for that specific panel.
    // can try - creating a game event for ui control schemes.

    //  when a button gets pressed that closes the current panel and opens a new one, need to do the following:
    // [X]  0) disable control scheme by setting it to null
    // [X]  1) play a button pressed audio clip (instant)
    // [X]  2) play a button pressed animation clip / particle system (over time)
    // [X]  3) WAIT for the animation to finish (or a specific time)
    // [X]  4) unload the current panel (over time)
    // [X]  5) WAIT for the current panel to unload
    // [X]  6) load the pending panel (over time)
    // [X]  7) WAIT for the new panel to load
    // [X]  8) select a default button to be selected (could be fixed, or based on save data - make it flexible)
    // [X]  9) update the control scheme to allow controls within the new window.

    // next type of animation: canvas group - alpha lerping.
    // Q: how to handle sequences of actions?

    // step 1: PROTOTYPE - create a mock system. emphasis: FUNCTIONALITY [done]
    // step 2: MODULARIZE - ensure each component is self-contained and loosely coupled.
    //                      this can involved refactoring all prototype scripts. Ensure all scripts
    //                      are still logically correct after refactoring. emphasis: SELF-CONTAINING LOGIC
    // step 3: TESTING - stress-test the system by coming up with new features and trying to implement them
    //                   using the current system. Ensure nothing breaks. emphasis: FLEXIBILITY AND STABILITY
    // step 4: OPTIMIZE* - this is optional. Only optimize if it's causing performance issues. USE THE PROFILER.

    // onto step 2 (modularizing):

    // problem 1: USING PRE-EXISTING SPAWNED IN ITEMS AND A MANAGER SCRIPT
    // currently, to play a sound, we depend on an FX player.
    // to play particles, we depend on a pre-existing particle system.
    // idea: USE EXCLUSIVELY PREFABS WHENEVER WE TRY TO SPAWN SOMETHING IN THE GAME. (optimization idea for spawning - OBJECT POOLS)

    // problem 2: reference to a UI animator.
    // idea: make the UI object be animatable, by giving it a new component to store all animations.
    // just like the animation system in Unity, use an identifier (string/int/other) for each animation.
    // NOTE - its possible to put the UI animator on the UI object itself instead of the HUD manager.
    // HOWEVER - in some cases, a button may want to animate something else. How to handle this?

    // need to think if its even worth separating this into its own system, or is it acceptable to have one part of a button use a dependency to animate other things.


}