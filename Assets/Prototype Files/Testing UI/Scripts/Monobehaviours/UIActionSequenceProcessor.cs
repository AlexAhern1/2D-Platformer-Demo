using System;
using UnityEngine;
using System.Collections.Generic;

namespace Game.UI
{
    public class UIActionSequenceProcessor : MonoBehaviour
    {
        // state variables
        private LinkedList<UIActionSequencePart> _currentSequence;
        private LinkedListNode<UIActionSequencePart> _currentPart;

        public void StartNewSequence(UIActionSequencePart[] sequence)
        {
            // convert the current sequence into a linked list.
            _currentSequence = new(sequence);

            _currentPart = _currentSequence.First;
            StartCurrentSequencePart();
        }

        private void OnCurrentSequencePartFinishWaiting()
        {
            // check if the current part has an awaiter.
            var awaiter = _currentPart.Value.Awaiter;

            if (awaiter == null)
            {
                Logger.Log("sequence complete.");
                return;
            }

            _currentPart = _currentPart.Next;
            StartCurrentSequencePart();
        }

        private void StartCurrentSequencePart()
        {
            _currentPart.Value.StartSequencePart(OnCurrentSequencePartFinishWaiting);
        }
    }

    [Serializable]
    public class UIActionSequencePart
    {
        // actions
        [SerializeReference, SubclassSelector]
        public UIAction[] Actions;

        [Separator]

        // awaiter
        [SerializeReference, SubclassSelector]
        public Awaiter Awaiter;

        // methods: start sequence:
        // actions.do action + awaiter.await (pass in method as argument)

        // on await: start executing the next set of actions (IF AWAITER ISN'T NULL)
        public void StartSequencePart(Action awaitCompleteCallback)
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                Actions[i].DoAction();
            }

            Awaiter?.Await(awaitCompleteCallback);
        }
    }

    [Serializable]
    public abstract class Awaiter
    {
        public abstract void Await(Action awaitCompleteCallback);
    }

    [Serializable]
    public class TimeAwaiter : Awaiter
    {
        public Timer Timer;
        public float WaitSeconds;

        public override void Await(Action awaitCompleteCallback)
        {
            Timer.StartTimer(WaitSeconds, awaitCompleteCallback);
        }
    }
}