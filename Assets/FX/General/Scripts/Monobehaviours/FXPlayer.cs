using UnityEngine;

namespace Game
{
    public class FXPlayer : MonoBehaviour
    {
        [SerializeField] private FXCollection[] FXCollectionArray;

        public void Play(int ID)
        {
            if (ID < 0 || ID >= FXCollectionArray.Length)
            {
                Logger.Error($"{this} OBJECT :tried to play an FX using an Invalid ID - {ID}. Length of collection: {FXCollectionArray.Length}", MoreColors.BrightRed);
                return;
            }

            var fx = FXCollectionArray[ID].FXs;

            for (int i = 0; i < fx.Length; i++) fx[i].Play();
        }
    }
}