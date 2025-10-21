using UnityEngine;

namespace Game.SaveData
{
    /// <summary>
    /// <para>This class should be attached to a game object that is in the main menu scene</para>
    /// </summary>
    public class SceneDataLoader : MonoBehaviour
    {
        [Header("scriptable objects")]
        //[SerializeField] private GameDataConfig _dataConfig;

        [Header("profile number")]
        [SerializeField] int profileNumber;

        private void OnValidate()
        {
            //if (_dataConfig == null) return;
            //_dataConfig.CurrentProfile = profileNumber;
        }

        private void Awake()
        {
            //_dataConfig.EnterPlaymode();
        }

        public void UpdateExistingProfiles()
        {
            //SaveFileHandler<GameData>.UpdateAllProfiles();
        }

        public void SaveGame()
        {
            //_dataConfig.RequestGameSave();
        }

        public void ReloadProfile()
        {
            //_dataConfig.LoadProfile(profileNumber);
        }
    }
}