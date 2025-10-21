using System;

namespace Game
{
    [Serializable]
    public class DamageBlockingConfig
    {
        public bool IsActive;
        public BlockMode Mode;
    }

    public enum BlockMode { Directional, FullCircle }
}