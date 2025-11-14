using System;
using UnityEngine;

namespace Game.UI
{
    [Serializable]
    public class LogMessageUIAction : UIAction
    {
        public string Message;
        public Color MessageColor = Color.white;

        public override void DoAction()
        {
            Logger.Log(Message, MessageColor);
        }
    }
}