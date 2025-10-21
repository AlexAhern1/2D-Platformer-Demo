using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Temp/Boss Encounter Data")]
    public class BossDataSO : ScriptableObject
    {
        [field: SerializeField] public bool Encountered { get; set; }
        [field: SerializeField] public bool Defeated { get; set; }
    }
}