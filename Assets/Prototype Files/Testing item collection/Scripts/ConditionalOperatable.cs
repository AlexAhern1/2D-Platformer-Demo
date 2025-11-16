using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class ConditionalOperatable : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text _textbox;

        [Header("Values")]
        [SerializeField] private Tag _permittedTag;
        [SerializeField] private string _conditionMetText;
        [SerializeField] private string _conditionNotMetText;

        [Header("SO Events")]
        [SerializeField] private GameEvent _operateInputEvent;

        [Header("Unity Events")]
        [SerializeField] private UnityEvent _operationUnityEvent;

        [Header("Condition")]
        [SerializeReference, SubclassSelector]
        private ICondition _operateCondition;

        // possible conditions:
        //  - enough currency
        //  - enough health
        //  - this item in inventory
        //  - this enemy beaten
        //  - this quest completed
        //  - this place visited
        //  - this upgrade unlocked
        //  - 100% something I can't think of (this is always true)

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
            if (!collision.CompareTag(_permittedTag)) return;

            else if (_operateCondition.Evaluate())
            {
                _operatable = true;
                _operateInputEvent.AddEvent(OnOperatePressed);
                _textbox.text = _conditionMetText;
            }

            else
            {
                _textbox.text = _conditionNotMetText;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.CompareTag(_permittedTag)) return;
            else if (!_operatable) _textbox.text = "";
            else
            {
                _operatable = false;
                _operateInputEvent.RemoveEvent(OnOperatePressed);
                _textbox.text = "";
            }
        }

        private void OnOperatePressed()
        {
            _operationUnityEvent.Invoke();
        }
    }
}