using UnityEngine;
using UnityEditor;

namespace Game.Variables
{
    [CustomPropertyDrawer(typeof(BoolReference))]
    public class BoolVariableDrawer : PropertyDrawer
    {
        private SerializedProperty _variable;
        private SerializedProperty _constantValue;
        private SerializedProperty _preference;

        private BoolVariable _boolVariable;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            //drawing prefix
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            //getting scriptable object variable as serialized property
            _variable = property.FindPropertyRelative("Variable");
            _boolVariable = (BoolVariable)_variable.objectReferenceValue;

            //drawing the box to drag and drop the scriptable object variable

            if (_boolVariable == null)
            {
                DrawLargeDropbox(position);
                EditorGUI.EndProperty();
                return;
            }
            DrawSmallDropbox(position);

            //getting the preference
            _preference = property.FindPropertyRelative("Preference");

            //drawing the value/constant preference dropdown on the inspector
            DrawPreferenceDropdown(position);

            //drawing the value rect based on the value of preference
            VariablePreference chosenPreference = (VariablePreference)_preference.enumValueIndex;
            DrawPreferredDisplay(position, chosenPreference, property);

            EditorGUI.EndProperty();
        }

        private void DrawSmallDropbox(Rect position)
        {
            Rect dropArea = GetBoxSize(position, 0.59f, 0.41f);
            EditorGUI.PropertyField(dropArea, _variable, GUIContent.none);
        }

        private void DrawLargeDropbox(Rect position)
        {
            Rect dropArea = GetBoxSize(position, 0.4f, 0.6f);
            EditorGUI.PropertyField(dropArea, _variable, GUIContent.none);
        }

        private void DrawPreferenceDropdown(Rect position)
        {
            Rect constButtonPosition = GetBoxSize(position, 0.4f, 0.16f);
            EditorGUI.PropertyField(constButtonPosition, _preference, GUIContent.none);
        }

        private void DrawPreferredDisplay(Rect position, VariablePreference preference, SerializedProperty property)
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

        private void DrawValueDisplay(Rect position)
        {
            GUI.enabled = false;
            EditorGUI.Toggle(GetValueRect(position), _boolVariable.Value);
            GUI.enabled = true;
        }

        private void DrawConstantDisplay(Rect position, SerializedProperty property)
        {
            _constantValue = property.FindPropertyRelative("ConstantValue");

            EditorGUI.PropertyField(GetValueRect(position), _constantValue, GUIContent.none);
        }

        private Rect GetBoxSize(Rect position, float xOffset, float xWidth)
        {
            float x = position.min.x + xOffset * position.width;
            float y = position.min.y;
            float width = position.width * xWidth;
            float height = EditorGUIUtility.singleLineHeight;

            return new Rect(x, y, width, height);
        }

        private Rect GetValueRect(Rect position)
        {
            return GetBoxSize(position, 0.56f, 0.135f);
        }
    }
}
