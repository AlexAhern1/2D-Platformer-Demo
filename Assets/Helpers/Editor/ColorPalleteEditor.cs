using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(ColorPallete))]
    public class ColorPalleteEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();


            if (GUILayout.Button("Generate Colors Class"))
            {
                var pallete = (ColorPallete)target;

                string scriptContents = "";
                scriptContents += "using UnityEngine;\n";
                scriptContents += "namespace Game {\n";
                scriptContents += $"public static class {pallete.CSFileName}" + "{\n";

                foreach (var c in pallete.Colors)
                {
                    string name = c.Name;

                    float r = c.Color.r;
                    float g = c.Color.g;
                    float b = c.Color.b;

                    string line = $"public static readonly Color {name} = new Color({r}f, {g}f, {b}f);\n";
                    scriptContents += line;
                }

                scriptContents += "}}";

                Logger.Log(scriptContents);
            }
        }
    }
}