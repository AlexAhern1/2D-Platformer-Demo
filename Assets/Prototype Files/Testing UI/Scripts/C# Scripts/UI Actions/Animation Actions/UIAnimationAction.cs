using UnityEngine;

namespace Game.UI
{
    [System.Serializable]
    public class UIAnimationAction : UIAction
    {
        [SerializeReference, SubclassSelector]
        public UIAnimation Animation;

        public override void DoAction()
        {
            Animation.Play();
        }
    }
}