using UnityEngine;

namespace Game
{
    public class GreyboxRectGetter : MonoBehaviour, IRectGetter
    {
        [SerializeField] private SpriteRenderer _renderer;

        public Rect GetRect()
        {
            return new Rect(transform.position, _renderer.size);
        }

        //private void OnDrawGizmosSelected()
        //{
        //    Vector2 BLcorner = transform.position;
        //    Vector2 size = _renderer.size;

        //    Logger.Log($"BL: {transform.position}");


        //    Rect r = new(BLcorner, size);

        //    Gizmos.color = Color.cyan;
        //    Gizmos.DrawWireCube(BLcorner, size);

        //    Gizmos.color = Color.red;
        //    Gizmos.DrawWireCube(r.position, r.size);
        //}
    }
}