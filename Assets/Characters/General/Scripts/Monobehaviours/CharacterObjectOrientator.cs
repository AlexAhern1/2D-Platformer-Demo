using UnityEngine;

namespace Game
{
    public class CharacterObjectOrientator : MonoBehaviour
    {
        [SerializeField] private Transform[] _objectTransforms;

        private const float RIGHT_ANGLE = 90f;

        public void Orientate(float direction)
        {
            if (direction == 0)
            {
                Logger.Error("Direction must be nonzero!");
                return;
            }

            direction = Mathf.Sign(direction);
            Quaternion angle = Quaternion.Euler(0, (1f - direction) * RIGHT_ANGLE, 0);
            for (int i = 0;  i < _objectTransforms.Length; i++)
            {
                _objectTransforms[i].rotation = angle;
            }
        }

        public void ANIM_EVENT_Orientate(float direction) => Orientate(direction);
    }
}