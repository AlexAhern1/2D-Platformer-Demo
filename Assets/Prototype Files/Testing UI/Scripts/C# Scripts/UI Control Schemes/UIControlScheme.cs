namespace Game.UI
{
    [System.Serializable]
    public abstract class UIControlScheme
    {
        public virtual void OnPressNavigate() { }

        public virtual void OnPressSelect() { }

        public virtual void OnPressBack() { }

        public virtual void OnPressCancel() { }
    }
}