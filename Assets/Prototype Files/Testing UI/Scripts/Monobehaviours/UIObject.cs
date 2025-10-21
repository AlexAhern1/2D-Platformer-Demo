using UnityEngine;

namespace Game.UI
{
    public class UIObject : MonoBehaviour
    {
        [Separator]
        [SerializeReference, SubclassSelector]
        private UIAction[] _initializeActions;

        [Separator]
        [SerializeReference, SubclassSelector]
        private UIAction[] _enterActions;

        [Separator]
        [SerializeReference, SubclassSelector]
        private UIAction[] _exitActions;

        [Separator]
        [SerializeReference, SubclassSelector]
        private UIAction[] _selectActions;

        public void Initialize()
        {
            DoActions(_initializeActions);
        }

        public void Enter()
        {
            DoActions(_enterActions);
        }

        public void Exit()
        {
            DoActions(_exitActions);
        }

        public void Select()
        {
            DoActions(_selectActions);
        }

        private void DoActions(UIAction[] actions)
        {
            if (actions == null) return;
            for (int i = 0; i < actions.Length; i++) actions[i].DoAction();
        }
    }
}