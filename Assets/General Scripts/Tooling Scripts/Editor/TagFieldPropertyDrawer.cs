using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomPropertyDrawer(typeof(Tag))]
    public class TagFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty tagProperty = property.FindPropertyRelative("Name");
            if (tagProperty == null)
            {
                Logger.Warn("Tag field property drawer error!", Logger.Refactoring, MoreColors.Scarlet);
            }

            // Calculate rect for the label
            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);

            // Draw label separately
            EditorGUI.PrefixLabel(labelRect, GUIUtility.GetControlID(FocusType.Passive), label);

            // Calculate rect for the dropdown menu
            Rect dropdownRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y, position.width - EditorGUIUtility.labelWidth, position.height);

            // Get all available tags in the project
            string[] allTags = UnityEditorInternal.InternalEditorUtility.tags;

            // Get the index of the currently selected tag
            int selectedIndex = GetSelectedIndex(tagProperty.stringValue, allTags);

            // Display a dropdown menu for selecting the tag
            selectedIndex = EditorGUI.Popup(dropdownRect, selectedIndex, allTags);

            // Update the serialized property with the selected tag
            tagProperty.stringValue = allTags[selectedIndex];

            EditorGUI.EndProperty();
        }

        // Get the index of the selected tag
        private int GetSelectedIndex(string selectedTag, string[] allTags)
        {
            for (int i = 0; i < allTags.Length; i++)
            {
                if (allTags[i] == selectedTag)
                    return i;
            }
            return 0; // Default to the first tag if the selected tag is not found
        }
    }
}