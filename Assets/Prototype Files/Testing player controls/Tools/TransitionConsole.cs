using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class TransitionConsole : MonoBehaviour
    {
        [SerializeField] TMP_Text _consoleText;
        [SerializeField] private float _textDuration;

        // collections
        private readonly Queue<StringQueue> _queue = new(16);

        public void AddText(string text)
        {
            if (text == "") return;

            _queue.Enqueue(new(text, _textDuration));
            AppendConsole(text);
        }

        private void FixedUpdate()
        {
            if (_queue.Count == 0 || Time.time < _queue.Peek().EndTime) return;
            RemoveFromConsole(_queue.Dequeue().Text);
        }

        private void AppendConsole(string text)
        {
            _consoleText.text += text + "\n";
        }

        private void RemoveFromConsole(string text)
        {
            _consoleText.text = _consoleText.text[(text.Length + 1)..];
        }
    }

    public struct StringQueue
    {
        public StringQueue(string text, float duration)
        {
            Text = text;
            EndTime = Time.time + duration;
        }

        public string Text;
        public float EndTime;
    }
}