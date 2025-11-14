using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    [System.Serializable]
    public class UIImageColorAction : UIAction
    {
        public Image[] Images;
        public Color Col;
        public bool DoThing = false;

        public override void DoAction()
        {
            for (int i = 0; i < Images.Length; i++) Images[i].color = Col;
        }
    }
}