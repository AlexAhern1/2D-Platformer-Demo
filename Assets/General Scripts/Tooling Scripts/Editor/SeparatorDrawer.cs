using UnityEngine;
using UnityEditor;

namespace Game.CustomInspector
{
    [CustomPropertyDrawer(typeof(Separator))] //important to declare which attribute is being used
    public class SeparatorDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            //get a reference to the attribute
            Separator separator = attribute as Separator;

            //define the line to draw
            float x = position.xMin;
            float y = position.yMin + (separator.Spacing + separator.Thickness) * 0.5f;
            float width = position.width;
            float height = separator.Thickness;

            Rect separatorRect = new Rect(x, y, width, height);

            //draw the line
            EditorGUI.DrawRect(separatorRect, separator.Color);
        }

        public override float GetHeight()
        {
            Separator separator = attribute as Separator;
            float height = separator.Spacing + 2 * separator.Thickness;
            return height;
        }
    }
}