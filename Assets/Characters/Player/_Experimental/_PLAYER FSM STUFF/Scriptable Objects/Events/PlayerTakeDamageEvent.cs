using UnityEngine;

namespace Game.Player
{
    [CreateAssetMenu(menuName = "SO/Game Events/Player Take Damage")]
    public class PlayerTakeDamageEvent : GameEvent<PlayerHealthChangeData> { }
}