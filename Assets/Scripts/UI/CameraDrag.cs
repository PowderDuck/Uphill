using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Uphill.Scripts.UI
{
    public class CameraDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [field: SerializeField]
        public float DragSensitivity { get; private set; } = 50f;
        [field: SerializeField]
        public float DragAdjustmentDelay { get; private set; } = 1.25f;
        [field: SerializeField]
        public float DragAdjustmentDuration { get; private set; } = 0.75f;
        [field: SerializeField]
        public float DragRadius { get; private set; } = 3;

        public event Action<Vector2>? Dragging;
        public event Action? BeginDrag;
        public event Action? EndDrag;

        private Vector2 _initialPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag?.Invoke();

            _initialPosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData) =>
            Dragging?.Invoke(eventData.position - _initialPosition);

        public void OnEndDrag(PointerEventData eventData) => EndDrag?.Invoke();
    }
}
