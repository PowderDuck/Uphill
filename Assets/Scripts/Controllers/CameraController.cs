using DG.Tweening;
using UnityEngine;
using Uphill.Scripts.UI;

namespace Uphill.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Climber _climber = default!;

        [SerializeField] private float _adjustmentVelocity = 5;
        [SerializeField] private Vector2 _boundaries = new(0.8f, 0.8f);

        [SerializeField] private CameraDrag _cameraDrag = default!;

        private Vector2 _cameraDimensions { get; set; } = Vector2.zero;

        private Vector2 _direction { get; set; } = Vector2.zero;
        private Vector3 _dynamicPosition = Vector3.zero;
        private bool _isAdjusting { get; set; } = false;

        private Tween _dragTween = default!;
        private Vector3 _initialOffset;
        private Vector3 _offset;

        private void Start()
        {
            var orthographicSize = Camera.main.orthographicSize;
            _cameraDimensions = new(
                (float)Camera.main.pixelWidth / Camera.main.pixelHeight * orthographicSize,
                orthographicSize);

            _dynamicPosition = transform.position;

            _cameraDrag.BeginDrag += OnBeginDrag;
            _cameraDrag.Dragging += OnDragging;
            _cameraDrag.EndDrag += OnEndDrag;
        }

        private void FixedUpdate()
        {
            _direction = _climber.transform.position - transform.position;

            // if (!_isAdjusting)
            // {
            //     _isAdjusting = (Mathf.Abs(_direction.x) / _cameraDimensions.x) >= _boundaries.x
            //         || (Mathf.Abs(_direction.y) / _cameraDimensions.y) >= _boundaries.y;
            // }
            // else
            // {
            //     _dynamicPosition.Set(
            //         _dynamicPosition.x + (_adjustmentVelocity * _direction.normalized.x),
            //         _dynamicPosition.y + (_adjustmentVelocity * _direction.normalized.y),
            //         _dynamicPosition.z);

            //     if (_direction.magnitude <= _adjustmentVelocity)
            //     {
            //         _dynamicPosition.Set(
            //             _climber.transform.position.x,
            //             _climber.transform.position.y,
            //             _dynamicPosition.z);

            //         _isAdjusting = false;
            //     }
            // }

            transform.position = _dynamicPosition + _offset;
        }

        private void OnBeginDrag()
        {
            _initialOffset = _offset;

            _dragTween?.Kill();
        }

        private void OnDragging(Vector2 offset)
        {
            _offset = (Vector2)_initialOffset - (offset / _cameraDrag.DragSensitivity);

            if (_offset.magnitude > _cameraDrag.DragRadius)
            {
                _offset = _offset.normalized * _cameraDrag.DragRadius;
            }
        }

        private void OnEndDrag()
        {
            _initialOffset = _offset;

            _dragTween = DOVirtual
                .DelayedCall(_cameraDrag.DragAdjustmentDelay, AdjustOffset);
        }

        private void AdjustOffset()
        {
            _dragTween = DOVirtual
                .Float(1, 0, _cameraDrag.DragAdjustmentDuration, OnDragAdjustment)
                .SetEase(Ease.OutExpo);
        }

        private void OnDragAdjustment(float percentage)
        {
            _offset = Vector3.Lerp(
                Vector3.zero, _initialOffset, percentage);
        }

        private void OnDestroy()
        {
            _cameraDrag.BeginDrag -= OnBeginDrag;
            _cameraDrag.Dragging -= OnDragging;
            _cameraDrag.EndDrag -= OnEndDrag;
        }
    }
}
