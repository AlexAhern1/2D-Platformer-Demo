using UnityEngine;

namespace Game
{
    public interface IFloatGetter
    {
        public float GetFloat();
    }

    [System.Serializable]
    public class FloatData : IFloatGetter
    {
        public float Value;
        
        public float GetFloat() => Value;
    }

    [System.Serializable]
    public class TransformDirection : IFloatGetter
    {
        public Transform T1;
        public Transform T2;

        public bool UseVerticalDirection;

        public float GetFloat() => MyExtensions.TrueSign(UseVerticalDirection ? T2.position.y - T1.position.y : T2.position.x - T1.position.x);
    }

    [System.Serializable]
    public class Vector2InputDirection : IFloatGetter
    {
        public Vector2InputEvent InputEvent;
        public bool UseVertical;

        public float GetFloat()
        {
            Vector2 value = InputEvent.InputValue;
            return UseVertical ? value.y : value.x;
        }
    }

    [System.Serializable]
    public class ModifiedFloatGetter : IFloatGetter
    {
        [Header("Float Getter")]
        [SerializeReference, SubclassSelector]
        public IFloatGetter Getter;

        [Header("Modifiers")]
        public bool UseAbs;
        public bool UseSign;
        public bool flipSign;

        public float GetFloat()
        {
            float value = Getter.GetFloat();
            if (UseAbs) value = Mathf.Abs(value);
            if (UseSign) value = MyExtensions.TrueSign(value);
            if (flipSign) value *= -1;
            return value;
        }
    }

    [System.Serializable]
    public class TimeFloatGetter : IFloatGetter
    {
        public bool UseUnscaledTime;

        public float GetFloat() => UseUnscaledTime ? Time.unscaledTime : Time.time;
    }

    [System.Serializable]
    public class AgentResourceGetter : IFloatGetter
    {
        public AgentResourceHandler Resources;
        public int ResourceID;

        public float GetFloat() => Resources.Get(ResourceID);
    }

    [System.Serializable]
    public class DualFloatGetter : IFloatGetter
    {
        [SerializeReference, SubclassSelector]
        public IFloatGetter Getter1;

        [SerializeReference, SubclassSelector]
        public IFloatGetter Getter2;

        public BinaryOperator Operator;

        public float GetFloat()
        {
            float f1 = Getter1.GetFloat();
            float f2 = Getter2.GetFloat();

            return Operator.Get(f1, f2);
        }
    }
}