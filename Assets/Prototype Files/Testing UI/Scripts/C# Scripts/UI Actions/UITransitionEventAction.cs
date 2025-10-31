using System;

namespace Game.UI
{
    [Serializable]
    public class UITransitionEventAction : UIAction
    {
        public PanelTransitionHandler TransitionHandler;
        public int TransitionID;


        public override void DoAction()
        {
            TransitionHandler.Transition(TransitionID);
        }
    }
}