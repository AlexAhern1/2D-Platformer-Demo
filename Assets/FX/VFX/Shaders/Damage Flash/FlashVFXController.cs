using System.Collections;
using UnityEngine;

namespace Game
{
    public class FlashVFXController : MonoBehaviour
    {
        [Header("For my lazy ass")]
        [SerializeField] private Material _material;

        [Header("Renderers")]
        [SerializeField] private SpriteRenderer[] _renderers;
        [SerializeField] private GameObject _rendersParent;

        [Header("Property names")]
        [SerializeField] private string _flashColorName;
        [SerializeField] private string _flashAmountName;

        [Header("VFX Config")]
        [SerializeField] private Color _flashColor;
        [SerializeField] private AnimationCurve _colorCurve;
        [SerializeField] private float _duration;

        private MaterialPropertyBlock _block;
        private Coroutine _flashCoroutine;

        private void Awake()
        {
            _block = new();
        }

        [ContextMenu("Populate Renderers Array")]
        public void PopulateRenderersArray()
        {
            _renderers = _rendersParent.GetComponentsInChildren<SpriteRenderer>();
            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].material = _material;
            }
        }

        [ContextMenu("Start Flash")]
        public void StartFlash()
        {
            StopFlash();
            _flashCoroutine = StartCoroutine(DoingFlash());
        }
        [ContextMenu("Stop Flash")]
        public void StopFlash()
        {
            if (_flashCoroutine != null)
            {
                StopCoroutine(_flashCoroutine);
                _flashCoroutine = null;

                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].GetPropertyBlock(_block);
                    _block.SetFloat(_flashAmountName, 0);
                    _renderers[i].SetPropertyBlock(_block);
                }
            }
        }

        private IEnumerator DoingFlash()
        {
            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].GetPropertyBlock(_block);
                _block.SetColor(_flashColorName, _flashColor);
                _renderers[i].SetPropertyBlock(_block);
            }

            float normTime = 0;
            float durationReciprocal = 1f / _duration;

            while (normTime < 1)
            {
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].GetPropertyBlock(_block);
                    _block.SetFloat(_flashAmountName, _colorCurve.Evaluate(normTime));
                    _renderers[i].SetPropertyBlock(_block);
                }
                normTime += Time.deltaTime * durationReciprocal;
                yield return null;
            }

            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].GetPropertyBlock(_block);
                _block.SetFloat(_flashAmountName, 0);
                _renderers[i].SetPropertyBlock(_block);
            }

            yield break;
        }
    }
}