using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomPropertyDrawer(typeof(TargetHolder))]
    public class TargetHolderPropertyDrawer : PropertyDrawer
    {
        private SerializedProperty _nameProp;
        private SerializedProperty _inspectorEditableProp;
        private SerializedProperty _targetProp;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            EditorGUI.BeginProperty(position, label, property);

            _nameProp = property.FindPropertyRelative("Name");
            _inspectorEditableProp = property.FindPropertyRelative("_inspectorEditable");
            _targetProp = property.FindPropertyRelative("_target");

            float x = position.min.x;
            float y = position.min.y;
            float ox = position.size.x * 0.38f;
            float dx = position.size.x;
            float dy = EditorGUIUtility.singleLineHeight;

            label = new GUIContent($"{GetElementIndex(property)} : {_nameProp.stringValue}");

            EditorGUI.PropertyField(new Rect(x, y, dx, dy), _nameProp, label);

            y += EditorGUIUtility.singleLineHeight;
            dx = 0;

            EditorGUI.PropertyField(new Rect(x + ox, y + 2f, dx, dy), _inspectorEditableProp, GUIContent.none);
            dx = position.size.x * 0.58f;
            ox = position.size.x * 0.42f;

            if (_inspectorEditableProp.boolValue)
            {
                EditorGUI.PropertyField(new Rect(x + ox, y + 2f, dx, dy), _targetProp, GUIContent.none);
            }
            else
            {
                GUI.enabled = false;
                EditorGUI.PropertyField(new Rect(x + ox, y + 2f, dx, dy), _targetProp, GUIContent.none);
                GUI.enabled = true;
            }

            EditorGUI.EndProperty();
        }

        private int GetElementIndex(SerializedProperty property)
        {
            string path = property.propertyPath; // e.g., "myArray.Array.data[3]"
            int start = path.IndexOf("[") + 1;
            int end = path.IndexOf("]");
            if (start > 0 && end > start)
            {
                string indexStr = path.Substring(start, end - start);
                if (int.TryParse(indexStr, out int index))
                    return index;
            }
            return -1; // not part of an array
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 2.2f * EditorGUIUtility.singleLineHeight;
        }
    }
}