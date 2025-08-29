using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Uphill.Scripts.Enums;
using Uphill.Scripts.Events;
using Uphill.Scripts.UI;

namespace Uphill.Scripts
{
    public class Climber : MonoBehaviour
    {
        [SerializeField] private List<LimbHitbox> _hitboxes = default!;

        [SerializeField] private Limb _leftArm = default!;
        [SerializeField] private Limb _rightArm = default!;
        [SerializeField] private Limb _leftLeg = default!;
        [SerializeField] private Limb _rightLeg = default!;

        [SerializeField] private float _adjustmentDuration = 0.75f;

        private Dictionary<LimbType, Limb> _limbs { get; set; } = default!;

        private Tweener _transformTweener { get; set; } = default!;

        private Vector3 _sourcePosition { get; set; } = Vector3.zero;
        private Vector3 _destinationPosition { get; set; } = Vector3.zero;

        private Quaternion _sourceRotation { get; set; } = Quaternion.identity;
        private Quaternion _destinationRotation { get; set; } = Quaternion.identity;

        private void Awake()
        {
            _limbs = new()
            {
                { LimbType.Left_Arm, _leftArm },
                { LimbType.Right_Arm, _rightArm },
                { LimbType.Left_Leg, _leftLeg },
                { LimbType.Right_Leg, _rightLeg },
            };

            // TODO: Should be handled inside GameManager
            foreach (var hitbox in _hitboxes)
            {
                hitbox.Initiated += OnHitboxInitiated;
            }

            foreach (var limb in _limbs.Values)
            {
                limb.Entered += OnLimbEntered;
            }
        }

        private void OnHitboxInitiated(LimbHitboxInitiatedEventArgs eventArgs)
        {
            if (_transformTweener != null && _transformTweener.IsActive())
            {
                return;
            }

            if (_limbs.TryGetValue(eventArgs.LimbType, out var limb))
            {
                limb.Stretch(eventArgs.Delta.normalized);
            }
        }

        private void OnLimbEntered(LimbEnteredEventArgs eventArgs)
        {
            if (_limbs.ContainsKey(eventArgs.LimbType))
            {
                var leftDirection = _leftArm.Grabber.transform.position - _leftLeg.Grabber.transform.position;
                var rightDirection = _rightArm.Grabber.transform.position - _rightLeg.Grabber.transform.position;

                _sourcePosition = transform.position;
                _destinationPosition = Vector3.zero;

                _sourceRotation = transform.rotation;
                _destinationRotation = Quaternion.LookRotation(
                    transform.forward,
                    ((leftDirection + rightDirection) / 2f).normalized);

                _transformTweener?.Kill();
                _transformTweener = DOVirtual
                    .Float(0, 1, _adjustmentDuration, UpdateTransform)
                    .SetEase(Ease.Linear);
            }
        }

        private void UpdateTransform(float percentage)
        {
            transform.SetPositionAndRotation(
                Vector3.Lerp(
                    _sourcePosition,
                    _destinationPosition,
                    percentage),
                Quaternion.Lerp(
                _sourceRotation,
                _destinationRotation,
                percentage));

            foreach (var limb in _limbs.Values)
            {
                limb.UpdateLimb();
            }
        }

        private void OnDestroy()
        {
            foreach (var hitbox in _hitboxes)
            {
                hitbox.Initiated -= OnHitboxInitiated;
            }

            foreach (var limb in _limbs.Values)
            {
                limb.Entered -= OnLimbEntered;
            }
        }
    }
}
