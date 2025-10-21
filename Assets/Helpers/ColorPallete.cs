using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "SO/Tools/Color Pallete")]
    public class ColorPallete : ScriptableObject
    {
        public ColorName[] Colors;
        public string CSFileName;
    }
}