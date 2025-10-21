using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// base class served as providing a method to detect colliders using casting methods.
    /// </summary>
    public abstract class Caster : MonoBehaviour
    {
        [SerializeField] protected bool visualizeLine;
        [SerializeField] private int _hitTargetSize = 4;

        private bool _targetsChanged;

        //filter to include layers that can be detected by the line cast
        [SerializeField] protected LayerMask targetMask;

        //linecast results
        protected RaycastHit2D[] hits;

        //list of raycasthit2d that only ever changes when the size of linecasthits changes or (another condition).
        protected List<Collider2D> lastUpdatedHits;

        //list of all targets currently within the linecast line.
        protected List<Collider2D> savedHits;

        private bool bothHitsAreEmpty => hits.Count() == 0 && lastUpdatedHits.Count == 0;
        private bool bothHitsAreNonEmpty => hits.Count() > 0 && lastUpdatedHits.Count > 0;

        private event Action<Collider2D> _colliderFoundEvent;
        private event Action<Collider2D> _colliderLostEvent;

        public void Toggle(bool toggleOn)
        {
            enabled = toggleOn;

            // this fixes a bug where if there's still a collider when disabling, re-enabling the caster later on won't trigger when entering the collider's hitbox.
            if (!toggleOn) savedHits.Clear();
        }

        public void AddDetectionEvents(Action<Collider2D> foundEventCallback, Action<Collider2D> lostEventCallback)
        {
            _colliderFoundEvent += foundEventCallback;
            _colliderLostEvent += lostEventCallback;
        }

        public void RemoveDetectionEvents(Action<Collider2D> foundEventCallback, Action<Collider2D> lostEventCallback)
        {
            _colliderFoundEvent -= foundEventCallback;
            _colliderLostEvent -= lostEventCallback;
        }

        private bool HasLineTargetsChanged()
        {
            if (bothHitsAreEmpty) return false;
            else if (bothHitsAreNonEmpty)
            {
                return CheckAllHitTargets();
            }
            return true;
        }

        private void Awake()
        {
            enabled = false;
            hits = new RaycastHit2D[_hitTargetSize];
            lastUpdatedHits = new List<Collider2D>();
            savedHits = new List<Collider2D>();
        }

        private void Update()
        {
            FindTargets();
            _targetsChanged = HasLineTargetsChanged();

            if (_targetsChanged)
            {
                lastUpdatedHits = hits.Where(hit => hit.collider != null).Select(hit => hit.collider).ToList();
            }

            if (lastUpdatedHits.Count == savedHits.Count && !_targetsChanged) return;

            else if (lastUpdatedHits.Count > savedHits.Count)
            {
                SearchForNewHit();
            }
            else if (lastUpdatedHits.Count < savedHits.Count)
            {
                SearchForOldHit();
            }
            else if (lastUpdatedHits.Count > 0 && savedHits.Count > 0 && _targetsChanged)
            {
                SearchForNewHit();
                SearchForOldHit();
            }
            
            //reset hits
            for (int i = 0; i < hits.Length; i++)
            {
                hits[i] = default;
            }
        }

        private bool CheckAllHitTargets()
        {
            if (hits.Length == lastUpdatedHits.Count)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (!lastUpdatedHits.Contains(hits[i].collider)) return true;
                }
                return false;
            }
            return true;
        }

        private void SearchForNewHit()
        {
            foreach (Collider2D hit in lastUpdatedHits)
            {
                if (!savedHits.Contains(hit))
                {
                    OnEnterLinecast(hit);
                }
            }
        }

        private void SearchForOldHit()
        {
            for (int i = savedHits.Count - 1; i >= 0; i--)
            {
                if (!lastUpdatedHits.Contains(savedHits[i]))
                {
                    OnExitLinecast(savedHits[i]);
                }
            }
        }

        private void OnEnterLinecast(Collider2D hit)
        {
            savedHits.Add(hit);
            _colliderFoundEvent?.Invoke(hit);
        }

        private void OnExitLinecast(Collider2D hit)
        {
            savedHits.Remove(hit);
            _colliderLostEvent?.Invoke(hit);
        }

        protected abstract void FindTargets();
    }
}