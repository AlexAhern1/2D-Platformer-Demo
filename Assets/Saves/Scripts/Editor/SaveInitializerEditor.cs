using UnityEngine;
using UnityEditor;
using Game.SaveData;

namespace Game
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(SceneDataLoader))]
    public class SaveInitializerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            SceneDataLoader dataLoader = (SceneDataLoader)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Update Profiles"))
            {
                dataLoader.UpdateExistingProfiles();
            }

            if (GUILayout.Button("Save Game"))
            {
                dataLoader.SaveGame();
            }

            if (GUILayout.Button("Reload Game"))
            {
                dataLoader.ReloadProfile();
            }
        }
    }
}