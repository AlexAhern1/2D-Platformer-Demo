using System.Collections;
using UnityEngine;
using UnityEditor;

namespace Game
{
    [CustomEditor(typeof(ProjectileSpawner))]
    public class ProjectileSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Aim and Fire"))
            {
                var s = (ProjectileSpawner)target;

                if (!Application.isPlaying)
                {
                    Logger.Warn("Must be in playmode to test ranged attacks.");
                    return;
                }
                else if (s.AimSource == null || s.DebugTarget == null || s.DebugForward == null)
                {
                    Logger.Warn($"at least one of the debugging fields is null - {s}");
                    return;
                }

                s.StartCoroutine(AimAndFire(s));
            }

            if (GUILayout.Button("Fire at Debug Target"))
            {
                ((ProjectileSpawner)target).FireAtDebugTarget();
            }
        }

        private IEnumerator AimAndFire(ProjectileSpawner spawner)
        {
            var t = spawner.DebugTarget;
            var f = spawner.DebugForward;
            var time = spawner.DebugAimDuration;

            float endTime = Time.time + time;
            while (Time.time < endTime)
            {
                spawner.Aim(t);
                yield return new WaitForFixedUpdate();
            }

            spawner.Fire();
        }
    }
}