using System;
using DG.Tweening;
using UnityEngine;
using Uphill.Scripts.Climbables;
using Uphill.Scripts.Consumables;
using Uphill.Scripts.Enums;
using Uphill.Scripts.Events;

namespace Uphill.Scripts
{
    public class Limb : MonoBehaviour
    {
        [SerializeField] private LimbType _limbType = LimbType.None;

        [SerializeField] private Transform _stretchableTransform = default!;
        [SerializeField] private float _stretchability = 5f;
        [SerializeField, Tooltip("Speed Per Second")] private float _stretchVelocity = 1f;

        [field: SerializeField]
        public Grabber Grabber { get; private set; } = default!;

        public event Action<LimbEnteredEventArgs>? Entered;

        private Vector3 _stretchLocalScale = Vector3.one;
        private Vector3 _stretchLocalPosition = Vector3.zero;

        private Vector3 _sourcePosition { get; set; } = Vector3.zero;
        private Vector3 _destinationPosition { get; set; } = Vector3.zero;

        private Tweener _limbTweener = default!;

        private void Start()
        {
            _sourcePosition = transform.position;
            _destinationPosition = transform.position;
        }

        public void Stretch(Vector3 direction)
        {
            if (_limbTweener == null || !_limbTweener.IsActive())
            {
                _sourcePosition = transform.position;
                _destinationPosition = transform.position + (_stretchability * direction);

                _limbTweener = DOVirtual
                    .Float(0, 1, _stretchability / _stretchVelocity, OnUpdateStretchable)
                    .SetEase(Ease.Linear)
                    .OnComplete(Unstretch);

                Grabber.ClimbableEntered -= OnClimbableEntered;
                Grabber.ClimbableEntered += OnClimbableEntered;

                Grabber.ConsumableEntered -= OnConsumableEntered;
                Grabber.ConsumableEntered += OnConsumableEntered;
            }
        }

        private void Unstretch()
        {
            _sourcePosition = Grabber.transform.position;
            _destinationPosition = transform.position;

            _limbTweener?.Kill();
            _limbTweener = DOVirtual
                .Float(0, 1, _stretchability / _stretchVelocity, OnUpdateStretchable)
                .SetEase(Ease.Linear);
        }

        public void UpdateLimb() => OnUpdateStretchable(1f);

        private void OnUpdateStretchable(float percentage)
        {
            var targetPoint = Vector3.Lerp(
                _sourcePosition, _destinationPosition, percentage);

            var direction = targetPoint - transform.position;

            transform.eulerAngles = new(
                0, 0, -Mathf.Atan2(direction.normalized.x, direction.normalized.y) * Mathf.Rad2Deg);

            _stretchLocalPosition.Set(0, direction.magnitude / 2f, 0);
            _stretchLocalScale.Set(
                _stretchableTransform.localScale.x,
                direction.magnitude / 2f,
                _stretchableTransform.localScale.z);

            _stretchableTransform.localPosition = _stretchLocalPosition;
            _stretchableTransform.localScale = _stretchLocalScale;

            Grabber.SetPosition(targetPoint, percentage);
        }

        private void OnClimbableEntered(Climbable climbable)
        {
            _limbTweener?.Kill();
            Grabber.ClimbableEntered -= OnClimbableEntered;

            _destinationPosition = climbable.transform.position;
            UpdateLimb();

            Entered?.Invoke(new(_limbType));
        }

        private void OnConsumableEntered(Consumable _)
        {
            Grabber.ConsumableEntered -= OnConsumableEntered;

            Unstretch();
        }

        private void OnDestroy()
        {
            Grabber.ClimbableEntered -= OnClimbableEntered;
            Grabber.ConsumableEntered -= OnConsumableEntered;
        }
    }
}
