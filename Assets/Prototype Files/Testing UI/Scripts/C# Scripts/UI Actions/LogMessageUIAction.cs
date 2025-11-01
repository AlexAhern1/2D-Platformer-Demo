using System;

namespace Game.UI
{
    [Serializable]
    public class LogMessageUIAction : UIAction
    {
        public string Message;

        public override void DoAction()
        {
            Logger.Log(Message);
        }
    }
}