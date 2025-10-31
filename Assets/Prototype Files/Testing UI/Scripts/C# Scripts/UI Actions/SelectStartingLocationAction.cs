using System;

namespace Game.UI
{
    [Serializable]
    public class SelectStartingLocationAction : UIAction
    {
        public StartingLocationSelector LocationSelector;
        public int LocationID;

        public override void DoAction()
        {
            LocationSelector.LoadStartingLocation(LocationID);
        }
    }
}