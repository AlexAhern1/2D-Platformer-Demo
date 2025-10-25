using System;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class FXCollection
    {
        public string InspectorName;
        [SerializeReference, SubclassSelector]
        public FXBase[] FXs;
    }

    [Serializable]
    public abstract class FXBase
    {
        public abstract void Play();

        public virtual void Stop() { }
    }

    [Serializable]
    public class AudioFX : FXBase
    {
        public AudioPlayer Audio;

        public override void Play()
        {
            Audio.Play();
        }
    }

    [Serializable]
    public class ParticleFX : FXBase
    {
        [Header("Prefab")]
        public ParticleSystem Particles;

        [Header("Parent component (optional)")]
        public GameObject Parent;

        [Header("Spawning config")]
        [SerializeReference, SubclassSelector]
        public IVector2Getter Spawnpoint;

        [SerializeReference, SubclassSelector]
        public IQuaternionProvider Rotation;

        public override void Play()
        {
            ParticleSystem particles;

            if (Parent != null)
            {
                particles = GameObject.Instantiate(Particles, Parent.transform);
                
                var transform = particles.transform;
                transform.SetLocalPositionAndRotation(Spawnpoint.Get(), Rotation.Get());
            }
            else
            {
                Logger.Log($"{Particles}, {Spawnpoint}, {Rotation}");
                particles = GameObject.Instantiate(Particles, Spawnpoint.Get(), Rotation.Get());
            }

            particles.Play();
        }
    }

    [Serializable]
    public class ShaderFX : FXBase
    {
        public FlashVFXController FlashController;

        public override void Play() => FlashController.StartFlash();
    }

    [Serializable]
    public class ScreenShakeFX : FXBase
    {
        public ScreenShakeData Data;
        public ScreenShakeEvent Event;

        public override void Play()
        {
            Event.Raise(Data);
        }
    }

    [Serializable]
    public class TimescaleFX : FXBase
    {
        public TimescaleHandler Handler;
        public float Timescale;
        public float Duration;

        public override void Play() => Handler.SetTimescale(Timescale, Duration);
    }
}