using UnityEngine;
using UnityEditor;

namespace Game
{
    public static class DrawerHelper
    {
        //returns a rect to draw fields onto.
        public static Rect GetRect(Rect position, float xOffset, float xWidthRatio, int line, float yOffset = 0f)
        {
            float x = position.min.x + xOffset * position.width;
            float y = position.min.y + (line - 1) * EditorGUIUtility.singleLineHeight + yOffset;
            float width = position.width * xWidthRatio;
            float height = EditorGUIUtility.singleLineHeight;

            return new Rect(x, y, width, height);
        }
    }
}