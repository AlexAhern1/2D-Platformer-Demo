namespace Game.UI
{
    [System.Serializable]
    public class UIGraphControlScheme : UIControlScheme
    {
        public UIPanelGraph PanelGraph;

        public override void OnPressNavigate()
        {
            PanelGraph.UpdateObjects();
        }

        public override void OnPressSelect()
        {
            PanelGraph.SelectCurrentObject();
        }
    }
}