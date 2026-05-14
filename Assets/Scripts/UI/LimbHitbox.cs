using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Uphill.Scripts.Events;

namespace Uphill.Scripts.UI
{
    public class LimbHitbox : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private float _activationRadius = 100f;

        private bool _isActive { get; set; } = false;

        private Vector2 _initialPosition = Vector2.zero;
        private DateTime _initiationTime = DateTime.Now;

        public event Action<LimbHitboxInitiatedEventArgs>? Initiated;

        public void OnPointerDown(PointerEventData eventData)
        {
            _initialPosition = eventData.position;
            _initiationTime = DateTime.Now;

            _isActive = true;
        }

        public void OnBeginDrag(PointerEventData eventData) { }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_isActive)
            {
                return;
            }

            var direction = eventData.position - _initialPosition;
            if (direction.magnitude >= _activationRadius)
            {
                Initiated?.Invoke(new(
                    direction.normalized,
                    _initialPosition,
                    (DateTime.Now - _initiationTime).TotalMilliseconds));

                _isActive = false;
            }
        }
    }
}
