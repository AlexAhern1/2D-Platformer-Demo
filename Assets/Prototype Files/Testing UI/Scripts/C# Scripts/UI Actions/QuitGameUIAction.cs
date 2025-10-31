using System;

namespace Game.UI
{
    [Serializable]
    public class QuitGameUIAction : UIAction
    {
        public GameQuitter quitter;

        public override void DoAction()
        {
            quitter.DoQuitGameSequence();
        }
    }
}