using UnityEngine;

namespace Game.World
{
    public class Door : MonoBehaviour
    {
        public void Close()
        {
            gameObject.SetActive(true);
        }

        public void Open()
        {
            gameObject.SetActive(false);
        }
    }
}