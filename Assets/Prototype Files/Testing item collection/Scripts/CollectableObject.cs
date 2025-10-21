using UnityEngine;

namespace Game
{
    public class CollectableObject : Spawnable
    {
        [SerializeField] private ItemData _data;
        [SerializeField] private ItemEvent _collectableEvent;

        [SerializeField] private bool _destroyOnCollect;
        [SerializeField] private GameObject _parentObject;

        public void Collect()
        {
            _collectableEvent.Raise(_data);
            if (_destroyOnCollect)
            {
                if (_parentObject == null) Destroy(gameObject);
                Destroy(_parentObject);
            }
        }

        public override void Spawn()
        {
            gameObject.SetActive(true);
        }

        public override void Despawn()
        {
            gameObject.SetActive(false);
        }
    }
}