using UnityEngine;

namespace Game
{
    public class NPCState : MonoBehaviour
    {
        public int ID { get; private set; }

        //  list of state behaviours.
        [SerializeReference, SubclassSelector]
        private StateBehaviour[] _behaviours;

        private float elapsedTime;

        public void SetID(int id) => ID = id;

        public void DebugBehaviourActivity()
        {
            foreach (var behaviour in _behaviours)
            {
                if (behaviour is not SearchForTargetBehaviour b) continue;

                b.DebugActivity = !b.DebugActivity;
            }
        }

        [ContextMenu("Find References")]
        public void FindBehaviourReferences()
        {
            for (int i = 0; i < _behaviours.Length; i++)
            {
                _behaviours[i].FindReferences();
            }
        }

        public void Enter()
        {
            gameObject.SetActive(true);

            elapsedTime = 0;

            for (int i = 0; i < _behaviours.Length; i++) _behaviours[i].Start();
        }

        public void Exit()
        {
            for (int i = 0; i < _behaviours.Length; i++) _behaviours[i].Stop();
            elapsedTime = 0;

            gameObject.SetActive(false);
        }

        /// <summary>
        /// must be called within a FixedUpdate callback.
        /// </summary>
        public void FixedRun()
        {
            for (int i = 0; i < _behaviours.Length; i++) _behaviours[i].FixedUpdate(elapsedTime);
            elapsedTime += Time.fixedDeltaTime;
        }
    }




}