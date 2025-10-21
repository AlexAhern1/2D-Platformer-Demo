using UnityEngine;

namespace Game
{
    public interface IToggle
    {
        public void Toggle(bool on);
    }

    [System.Serializable]
    public class ComponentToggle : IToggle
    {
        public MonoBehaviour Component;

        public void Toggle(bool on)
        {
            if (Component is IToggle toggle) toggle.Toggle(on);
            else Logger.Warn($"Attached Component {Component} does not use the IToggle interface!", MoreColors.LightOrange);
        }
    }
}