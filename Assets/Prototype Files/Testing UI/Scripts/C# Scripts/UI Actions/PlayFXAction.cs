using UnityEngine;

namespace Game.UI
{
    [System.Serializable]
    public class PlayFXAction : UIAction
    {
        [SerializeReference, SubclassSelector]
        public FXBase Effect;

        public override void DoAction()
        {
            Effect.Play();
        }
    }
}