using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class DamageNumberUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textBox;
        [SerializeField] private float _displaySeconds;

        private Coroutine _displayingCoroutine;

        private void Awake()
        {
            _textBox.enabled = false;
        }

        public void DisplayDamageNumber(float number)
        {
            _textBox.enabled = true;
            if (_displayingCoroutine != null)
            {
                StopCoroutine(_displayingCoroutine);
            }

            _textBox.text = number.ToString();
            _displayingCoroutine = StartCoroutine(DisplayForSeconds(_displaySeconds));
        }

        IEnumerator DisplayForSeconds(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _textBox.enabled = false;
        }
    }
}