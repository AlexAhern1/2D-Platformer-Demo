using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(AttackController), true)]
    [CanEditMultipleObjects]
    public class AttackControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            AttackController controller = (AttackController)target;

            if (GUILayout.Button("Attack"))
            {
                controller.UseAttack();
            }
        }
    }
}