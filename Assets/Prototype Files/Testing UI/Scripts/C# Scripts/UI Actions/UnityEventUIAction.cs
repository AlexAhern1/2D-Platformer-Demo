using System;
using UnityEngine.Events;

namespace Game.UI
{
    [Serializable]
    public class UnityEventUIAction : UIAction
    {
        public UnityEvent PressEvent;

        public override void DoAction()
        {
            PressEvent.Invoke();
        }
    }
}
