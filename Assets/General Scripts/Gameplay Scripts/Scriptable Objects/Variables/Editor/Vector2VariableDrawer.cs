using UnityEditor;
using UnityEngine;

namespace Game.Variables
{
    [CustomPropertyDrawer(typeof(Vector2Reference))]
    public class Vector2VariableDrawer : PropertyDrawer
    {
        SerializedProperty variable;
        SerializedProperty constantValue;
        SerializedProperty preference;

        Vector2Variable vector2Variable;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            //drawing the prefix
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            //getting the scriptable object variable as a serialized property
            variable = property.FindPropertyRelative("Variable");
            vector2Variable = (Vector2Variable)variable.objectReferenceValue;

            //drawing the box to drag and drop the scriptable object variable

            if (vector2Variable == null)
            {
                DrawLargeDropbox(position);
                EditorGUI.EndProperty();
                return;
            }
            DrawSmallDropbox(position);

            //getting the preference
            preference = property.FindPropertyRelative("Preference");

            //drawing the value/constant preference dropdown on the inspector
            DrawPreferenceDropdown(position);

            //drawing the value rect based on the value of preference
            VariablePreference chosenPreference = (VariablePreference)preference.enumValueIndex;
            DrawPreferredDisplay(position, chosenPreference, property);

            EditorGUI.EndProperty();
        }

        void DrawSmallDropbox(Rect position)
        {
            Rect dropArea = GetBoxSize(position, 0.85f, 0.15f);
            EditorGUI.PropertyField(dropArea, variable, GUIContent.none);
        }

        void DrawLargeDropbox(Rect position)
        {
            Rect dropArea = GetBoxSize(position, 0.4f, 0.6f);
            EditorGUI.PropertyField(dropArea, variable, GUIContent.none);
        }

        void DrawPreferenceDropdown(Rect position)
        {
            Rect constButtonPosition = GetBoxSize(position, 0.4f, 0.16f);
            EditorGUI.PropertyField(constButtonPosition, preference, GUIContent.none);
        }

        void DrawPreferredDisplay(Rect position, VariablePreference preference, SerializedProperty property)
        {
            if (preference == VariablePreference.Value)
            {
                DrawValueDisplay(position);
                return;
            }
            else if (preference == VariablePreference.Constant)
            {
                DrawConstantDisplay(position, property);
            }
        }

        void DrawValueDisplay(Rect position)
        {
            GUI.enabled = false;
            Rect valuePosition = GetBoxSize(position, 0.14f, 1f);

            EditorGUI.Vector2Field(GetValueRect(valuePosition), "", vector2Variable.Value);
            GUI.enabled = true;
        }

        void DrawConstantDisplay(Rect position, SerializedProperty property)
        {
            constantValue = property.FindPropertyRelative("ConstantValue");

            EditorGUI.PropertyField(GetValueRect(position), constantValue, GUIContent.none);
        }

        Rect GetBoxSize(Rect position, float xOffset, float xWidth)
        {
            float x = position.min.x + xOffset * position.width;
            float y = position.min.y;
            float width = position.width * xWidth;
            float height = EditorGUIUtility.singleLineHeight;

            return new Rect(x, y, width, height);
        }

        Rect GetValueRect(Rect position)
        {
            return GetBoxSize(position, 0.42f, 0.28f);
        }
    }
}