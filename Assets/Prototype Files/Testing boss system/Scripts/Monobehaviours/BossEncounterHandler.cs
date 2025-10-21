using UnityEngine;

namespace Game.Enemy
{
    public class BossEncounterHandler : MonoBehaviour
    {
        [Header("First encounter objects")]
        [SerializeField] private InterfaceReference<GameObject, IGameEvent<GameObject>> _firstEncounterEvent;

        [Header("Subsequent encounter objects")]
        [SerializeField] private InterfaceReference<GameObject, IGameEvent<GameObject>> _subsequentEncounterEvent;

        [Header("Events")]
        [SerializeField] private GameEvent _enableMenusMapEvent;
        [SerializeField] private GameEvent _enableLevelMapEvent;
        [SerializeField] private BoolEvent _enableCustceneEvent;
        [SerializeField] private GameEvent _cutsceneOverEvent;

        [Header("Config")]
        [SerializeField] BossDataSO _bossData;
        [SerializeField] private int _bossFollowTargetID;

        [Header("References")]
        [SerializeField] private BossController _controller;
        [SerializeField] private TargetHandler _targetHandler;

        public void StartEncounter(bool first)
        {
            if (first)
            {
                _bossData.Encountered = true;

                _firstEncounterEvent.Object.SetActive(false);
                Logger.Log("first encounter.");
            }
            else
            {
                _subsequentEncounterEvent.Object.SetActive(false);
                Logger.Log("subsequent encounter.");
            }
        }

        private void Awake()
        {
            _controller.Initialize();
        }

        private void OnEnable()
        {
            if (_bossData.Encountered)
            {
                _subsequentEncounterEvent.Interface.AddEvent(OnSubsequentEncounter);
                _firstEncounterEvent.Object.SetActive(false);

                _controller.Enable();
            }
            else
            {
                _firstEncounterEvent.Interface.AddEvent(OnFirstEncounter);
                _subsequentEncounterEvent.Object.SetActive(false);
            }
        }

        private void OnDisable()
        {
            if (_bossData.Encountered)
            {
                _subsequentEncounterEvent.Interface.RemoveEvent(OnSubsequentEncounter);
            }
            else
            {
                _firstEncounterEvent.Interface.RemoveEvent(OnFirstEncounter);
            }
        }

        private void OnFirstEncounter(GameObject player)
        {
            _bossData.Encountered = true;

            _firstEncounterEvent.Interface.RemoveEvent(OnFirstEncounter);

            _firstEncounterEvent.Object.SetActive(false);
            Logger.Log("first encounter.");
        }

        private void OnSubsequentEncounter(GameObject player)
        {
            _subsequentEncounterEvent.Object.SetActive(false);
            Logger.Log("subsequent encounter.");

            // check if there's a cutscene to play.
            //  if there is, wait for the cutscene to end before continuing this method (ie - await)
            //  if there isn't, then just continue.

            StartBossFight(player);
        }

        private void StartBossFight(GameObject player)
        {
            // provide the enemy with the player's gameobject (target handler)
            _targetHandler.SetTarget(player, _bossFollowTargetID);

            // enable the health bar
            _controller.EnableHealthMediator();
        }
    }

    // 



}