using UnityEngine;

namespace Game.UI
{
    [System.Serializable]
    public class UIParticlesAction : UIAction
    {
        public ParticleSystem ParticleSystem;
        public bool IsPrefab;

        public override void DoAction()
        {
            if (IsPrefab)
            {
                // instantiate and spawn
            }
            else
            {
                ParticleSystem.Play();
            }
        }
    }
}