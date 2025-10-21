using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class QuitHandler : MonoBehaviour
    {
        [SerializeField] private Button _quitButton;

        private void OnEnable()
        {
            _quitButton.onClick.AddListener(OnClickQuit);
        }

        private void OnDisable()
        {
            _quitButton.onClick.RemoveListener(OnClickQuit);
        }

        private void OnClickQuit()
        {
            Logger.Log("quitting...");
            Application.Quit();
        }
    }
}