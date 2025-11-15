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
            for (int i = 0; i < Images.Length; i++)
            {
                var img = Images[i];
                img.color = Col;
                img.GraphicUpdateComplete();
                img.color = Col;
                if (DoThing) Logger.Log($"DID A THING", img.color);
            }
        }
    }
}