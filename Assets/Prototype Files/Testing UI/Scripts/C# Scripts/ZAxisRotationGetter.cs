using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class ZAxisRotationGetter : IQuaternionProvider
    {
        public float Degrees;

        public Quaternion Get() => Quaternion.Euler(0, 0, Degrees);
    }
}