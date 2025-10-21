namespace Game.UI
{
    [System.Serializable]
    public class InitializePanelAction : UIAction
    {
        public UIPanelGraph Panel;

        public override void DoAction()
        {
            Panel.Initialize();
        }
    }
}