using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(fileName = "Inventory", menuName = "SO/Managers/Inventory")]
    public class InventorySO : ScriptableObject
    {
        [SerializeField] private bool _resetOnRecompile;
        [SerializeField] private List<ItemData> _serializedContents = new();

        private readonly Dictionary<string, int> _contents = new();

        public Dictionary<string, int> contents => _contents;

        private void OnEnable()
        {
            if (_resetOnRecompile) Clear();

            for (int i = 0; i < _serializedContents.Count; i++)
            {
                _contents[_serializedContents[i].Name] = _serializedContents[i].Amount;
            }
        }

        public void Clear()
        {
            _serializedContents.Clear();
            if (_contents != null) _contents.Clear();
        }

        public void Add(ItemData data)
        {
            Add(data.Name, data.Amount);
        }

        public void Add(string name, int amount)
        {
            if (_contents.ContainsKey(name))
            {
                _contents[name] += amount;

                int index = _serializedContents.FindIndex(i => i.Name.Equals(name));
                _serializedContents[index] = new ItemData() { Name = name, Amount = _contents[name] };
                Logger.Log($"{this}: {name}: {_contents[name] - amount} -> {_contents[name]}");
            }
            else
            {
                Logger.Log($"{this}: {name}: {0} -> {amount}");
                _contents.Add(name, amount);

                _serializedContents.Add(new ItemData() { Name = name, Amount = _contents[name] });
            }
        }

        public bool TryRemove(string name, int amount)
        {
            if (!_contents.ContainsKey(name))
            {
                Logger.Warn($"No such item named {name} in {this}.");
                return false;
            }
            else if (_contents[name] < amount)
            {
                Logger.Warn($"Cannot remove {amount} {name}(s) from {this} as it only contains {_contents[name]}.");
                return false;
            }
            else
            {
                _contents[name] -= amount;

                int index = _serializedContents.FindIndex(i => i.Name.Equals(name));
                _serializedContents[index] = new ItemData() { Name = name, Amount = _contents[name] };

                Logger.Log($"{this}: {name}: {_contents[name] + amount} -> {_contents[name]}");

                return true;
            }
        }
    }
}