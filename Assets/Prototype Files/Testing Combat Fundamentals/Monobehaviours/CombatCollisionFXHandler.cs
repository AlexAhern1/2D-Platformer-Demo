using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class CombatCollisionFXHandler
    {
        // this will be given to each handle damage behaviour class.
        [SerializeField] private FXPlayer _fxPlayer;
        [Separator]
        [SerializeField] private SerializedDictionary<CombatCollisionResult, int> _fxIDs;

        public void PlayCombatCollisionFX(CombatCollisionResult result)
        {
            if (!_fxIDs.ContainsKey(result)) return;
            _fxPlayer.Play(_fxIDs[result]);
        }
    }
}