using UnityEngine;

namespace Game
{
    public class ConditionsHandler : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        public AttackCondition[] Conditions;

        public ICondition Get(int ID) => Conditions[ID];
    }
}