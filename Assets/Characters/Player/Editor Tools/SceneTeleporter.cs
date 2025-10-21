using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace Game
{
    [ExecuteInEditMode]
    public class SceneTeleporter : MonoBehaviour
    {
        [SerializeField] private GameObject _playerObject;
        [SerializeField] private KeyCode _teleportKeycode;

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnScene;
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnScene;
        }

        void OnScene(SceneView scene)
        {
            Event e = Event.current;

            if (!(e.type == EventType.KeyDown && e.keyCode == _teleportKeycode)) return;

            Vector2 mousePos = e.mousePosition;
            var ray = HandleUtility.GUIPointToWorldRay(mousePos);

            Plane plane = new Plane(Vector3.forward, Vector3.zero); // Z = 0 plane

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 worldPos = ray.GetPoint(distance);
                _playerObject.transform.position = worldPos;
            }

            // Prevent Unity from handling this event
            e.Use();
        }
    }
}

#endif