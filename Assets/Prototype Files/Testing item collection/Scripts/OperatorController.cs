using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Game.World
{
    [RequireComponent(typeof(Collider2D))]
    public class OperatorController : MonoBehaviour
    {
        [SerializeField] private Tag _permittedTag;
        [SerializeField] private GameEvent _operateInputEvent;

        // TEMPORARY UNITY EVENTS WILL BE USED.
        //  AS MORE AND MORE USE CASES FOR OPERATORS ARE FOUND, THEY WILL BECOME THEIR OWN CLASSES WHICH WILL REPLACE ALL UNITY EVENTS.
        [SerializeField] private UnityEvent _operationUnityEvent;

        [Header("Overhead text")]
        [SerializeField] private TMP_Text _textbox;

        private bool _operatable;

        private void OnDisable()
        {
            if (_operatable)
            {
                _operateInputEvent.RemoveEvent(OnOperatePressed);
            }
            _operatable = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(_permittedTag))
            {
                _operatable = true;
                _operateInputEvent.AddEvent(OnOperatePressed);
                _textbox.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(_permittedTag))
            {
                _operatable = false;
                _operateInputEvent.RemoveEvent(OnOperatePressed);
                _textbox.gameObject.SetActive(false);
            }
        }

        private void OnOperatePressed()
        {
            _operationUnityEvent.Invoke();
        }
    }
}