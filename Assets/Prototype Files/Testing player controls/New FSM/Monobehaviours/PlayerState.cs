using UnityEngine;

namespace Game.Player
{
    public class PlayerState : MonoBehaviour
    {
        [SerializeField] private StateBehaviour[] _behaviours;

        [Header("Visual Tools")]
        [Separator]
        [Header("Transition Logs")]
        public TransitionConsole Console;
        public bool LogTransition;
        public string EnterMessage;
        public string ExitMessage;

        public void Enter()
        {
            gameObject.SetActive(true);
            for (int i = 0; i < _behaviours.Length; i++) _behaviours[i].Enter();

            if (LogTransition) Console.AddText(EnterMessage);
        }

        public void Exit()
        {
            for (int i = 0; i < _behaviours.Length; i++) _behaviours[i].Exit();
            gameObject.SetActive(false);

            if (LogTransition) Console.AddText(ExitMessage);
        }
    }

    // where will the static resource be used?

    //  1) in static attack transition conditions:
    //  compare value if the amount of static is enough (more or equal to a specified value)

    //  2) in a monobehaviour class that reduces static by a certain amount (can toggle on/off)

    //  3) in a state when we need to ensure that the player has enough static.
    //  it is probably alright if we only have a "static reach zero" event.

    //  inside this monobehaviour, we can listen for when the static value reaches zero and invoke an event that way.

    // REMEMBER: READING: don't need resource, just the float variable
    //           WRITING: need the resource.

    //  since the player's static is being updated by numerous states (unlike health, which is a separate thing),
    //  
}