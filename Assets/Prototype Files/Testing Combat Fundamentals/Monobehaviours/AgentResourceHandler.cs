using UnityEngine;

namespace Game
{
    public class AgentResourceHandler : MonoBehaviour, IInitializable
    {
        [SerializeField] private AgentResource[] _resources;

        public void Initialize()
        {
            for (int i = 0; i < _resources.Length; i++) _resources[i].Initialize();
        }

        public void Set(int ID, float value) => _resources[ID].Set(value);

        public float Get(int ID) => _resources[ID].Value;
    }

    [System.Serializable]
    public class AgentResource
    {
        public string InspectorName;
        public float InitialValue;
        [SerializeField][ReadOnly] private float _value;

        public float Value => _value;

        public void Initialize() => _value = InitialValue;

        public void Set(float value)
        {
            _value = value;
        }

        [System.Obsolete]
        public void Add(float amount)
        {
            _value += amount;
        }
    }

    [System.Serializable]
    public abstract class ResourceSetter
    {
        public int ResourceID;

        public abstract void Set(AgentResourceHandler handler); //might be a good idea to use an interface for agent resource handler.
    }

    [System.Serializable]
    public class SetResource : ResourceSetter
    {
        public float Value;
        public float Range;

        public override void Set(AgentResourceHandler handler)
        {
            float value = Random.Range(Value - Range / 2, Value + Range / 2);
            handler.Set(ResourceID, value);
        }
    }

    [System.Serializable]
    public class AddResource : ResourceSetter
    {
        public float Value;
        public float Range;

        public override void Set(AgentResourceHandler handler)
        {
            float value = Random.Range(Value - Range / 2, Value + Range / 2);
            handler.Set(ResourceID, handler.Get(ResourceID) + value);
        }
    }

    [System.Serializable]
    public class AddToTimeResource : ResourceSetter
    {
        public float Value;
        public float Range;
        public bool UseUnscaledTime;

        public override void Set(AgentResourceHandler handler)
        {
            float value = Random.Range(Value - Range / 2, Value + Range / 2);
            float time = UseUnscaledTime ? Time.unscaledTime : Time.time;

            handler.Set(ResourceID, time + value);
        }
    }
}