using UnityEngine;

namespace Game
{
    /// <summary>
    /// used for any spawnable object that when spawned, will have some physics settings to it.
    /// </summary>
    [System.Serializable]
    public struct SpawnablePhysics
    {
        [Header("initial position")]
        public float MinSpawnDistance;
        public float MaxSpawnDistance;

        [Header("initial velocity")]
        public float MinVelocityMagnitude;
        public float MaxVelocityMagnitude;
        public float AngleCenter;
        public float AngleRange;

        [Header("initial torque")]
        public float MinTorque;
        public float MaxTorque;

        public Vector2 GetRandomSpawnpoint(Vector2 origin)
        {
            //get random distance
            float randomDist = Random.Range(MinSpawnDistance, MaxSpawnDistance);

            return origin + Random.insideUnitCircle * randomDist;
        }

        public Vector2 GetRandomVelocity()
        {
            //get random magnitude
            float randomMagnitude = Random.Range(MinVelocityMagnitude, MaxVelocityMagnitude);

            //get random angle
            float randomAngle = Mathf.Deg2Rad * Random.Range(AngleCenter + 0.5f * AngleRange, AngleCenter - 0.5f * AngleRange);

            float xComponent = randomMagnitude * Mathf.Cos(randomAngle);
            float yComponent = randomMagnitude * Mathf.Sin(randomAngle);

            return new Vector2(xComponent, yComponent);
        }

        public float GetRandomTorque(float hitDirection = 0)
        {
            if (hitDirection > 0)
            {
                return Random.Range(MinTorque, MaxTorque);
            }
            else if (hitDirection < 0)
            {
                return Random.Range(-MaxTorque, -MinTorque);
            }
            float averageTorque = 0.5f * (MaxTorque - MinTorque);
            return Random.Range(-averageTorque, averageTorque);
        }
    }
}