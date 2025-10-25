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

        // need an array of shader VFX configs so that we can select from a bunch.

        [Header("VFX Config")]
        [SerializeField] private FlashConfig[] _flashes;

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

        public void StartFlash(int flashID)
        {
            StopCurrentFlash();
            _flashCoroutine = StartCoroutine(DoingFlash(flashID));
        }

        public void StartFlash()
        {
            Logger.Warn("OUTDATED FLASH METHOD - USE NEW ONE", MoreColors.Amber);
            //StopFlash();
            //_flashCoroutine = StartCoroutine(DoingFlash());
        }

        public void StopCurrentFlash()
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

        private IEnumerator DoingFlash(int flashID)
        {
            var selectedFlash = _flashes[flashID];
            var flashColor = selectedFlash.FlashColor;
            var alphaCurve = selectedFlash.AlphaCurve;
            var duration = selectedFlash.Duration;

            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].GetPropertyBlock(_block);
                _block.SetColor(_flashColorName, flashColor);
                _renderers[i].SetPropertyBlock(_block);
            }

            float normTime = 0;
            float durationReciprocal = 1f / duration;

            while (normTime < 1)
            {
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].GetPropertyBlock(_block);
                    _block.SetFloat(_flashAmountName, alphaCurve.Evaluate(normTime));
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