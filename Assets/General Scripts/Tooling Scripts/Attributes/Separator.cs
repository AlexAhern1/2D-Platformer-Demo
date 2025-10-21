using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// attribute for creating a horizontal line in an inspector window
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class Separator : PropertyAttribute
    {
        public readonly float Spacing;
        public readonly float Thickness;
        public readonly Color Color;

        public Separator(float thickness = 1, float spacing = 10)
        {
            Spacing = spacing;
            Thickness = thickness;
            Color = new Color(1f, 1f, 1f, 0.5f);
        }
    }
}