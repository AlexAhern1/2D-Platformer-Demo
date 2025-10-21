using System;

namespace Game
{
    /// <summary>
    /// used by game objects that are required by traps
    /// </summary>
    public interface ITrapComponent
    {
        public void Entrap();

        public Action ReleaseEvent { get; set; }
    }
}