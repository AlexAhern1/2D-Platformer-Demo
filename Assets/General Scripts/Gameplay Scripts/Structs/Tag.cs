using System;

namespace Game
{
    [Serializable]
    public struct Tag
    {
        public string Name;

        public static implicit operator string(Tag tagField) { return tagField.Name; }
    }
}