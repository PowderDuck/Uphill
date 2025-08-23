using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Uphill.Scripts.Enums;
using Uphill.Scripts.Events;

namespace Uphill.Scripts.UI
{
    public class LimbHitbox : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler
    {
        [SerializeField] private LimbType _limbType = LimbType.None;
        [SerializeField] private float _activationRadius = 100f;

        private bool _isActive { get; set; } = false;

        private Vector2 _initialPosition = Vector2.zero;

        public Action<LimbHitboxInitiatedEventArgs> Initiated;

        public void OnPointerDown(PointerEventData eventData)
        {
            _initialPosition = eventData.position;
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
                Initiated?.Invoke(new(_limbType, direction.normalized));
                _isActive = false;
            }
        }
    }
}
