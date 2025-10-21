using UnityEngine;
using UnityEditor;
using Game.Enemy;

namespace Game
{
    [CustomEditor(typeof(EnemySpawnableOLD))]
    public class GuidGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Generate Guid"))
            {
                ((EnemySpawnableOLD)target).GenerateGuid();
                EditorUtility.SetDirty(target);
            }
        }
    }
}