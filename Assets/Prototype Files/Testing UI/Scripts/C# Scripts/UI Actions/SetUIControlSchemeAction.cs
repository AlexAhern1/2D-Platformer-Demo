using UnityEngine;

namespace Game.UI
{
    [System.Serializable]
    public class SetUIControlSchemeAction : UIAction
    {
        public UIControlSchemeEvent Event;

        [SerializeReference, SubclassSelector]
        public UIControlScheme ControlScheme;

        public override void DoAction()
        {
            Event.Raise(ControlScheme);
        }
    }
}