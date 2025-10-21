using Game.Player;
using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(PlayerDamageable))]
    public class PlayerDamageableEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            PlayerDamageable dmg = (PlayerDamageable)target;

            if (GUILayout.Button("Self-Inflict Damage"))
            {
                dmg.SelfInflictDamage();
            }
        }
    }
}