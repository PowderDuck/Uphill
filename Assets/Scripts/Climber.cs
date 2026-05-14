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
        [SerializeField] private LimbHitbox _hitbox = default!;

        [SerializeField] private Limb _leftArm = default!;
        [SerializeField] private Limb _rightArm = default!;
        [SerializeField] private Limb _leftLeg = default!;
        [SerializeField] private Limb _rightLeg = default!;

        [SerializeField] private float _adjustmentDuration = 0.75f;
        [SerializeField] private AnimationCurve _positionCurve = default!;
        [SerializeField] private AnimationCurve _rotationCurve = default!;

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

            _hitbox.Initiated += OnHitboxInitiated;

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

            var closestLimb = default(Limb);
            var closestDistance = -1f;
            foreach (var limb in _limbs.Values)
            {
                var direction = eventArgs.InitialPosition
                    - (Vector2)Camera.main.WorldToScreenPoint(limb.transform.position);

                if (closestDistance < 0 || direction.magnitude < closestDistance)
                {
                    closestDistance = direction.magnitude;
                    closestLimb = limb;
                }
            }

            closestLimb?.Stretch(eventArgs.Delta.normalized);
        }

        private void OnLimbEntered(LimbEnteredEventArgs eventArgs)
        {
            if (!_limbs.ContainsKey(eventArgs.LimbType))
            {
                return;
            }

            var leftDirection = _leftArm.Grabber.transform.position - _leftLeg.Grabber.transform.position;
            var rightDirection = _rightArm.Grabber.transform.position - _rightLeg.Grabber.transform.position;

            _sourcePosition = transform.position;
            _destinationPosition = GetOffsetPosition();

            _sourceRotation = transform.rotation;
            _destinationRotation = Quaternion.LookRotation(
                transform.forward,
                ((leftDirection + rightDirection) / 2f).normalized);

            _transformTweener?.Kill();
            _transformTweener = DOVirtual
                .Float(0, 1, _adjustmentDuration, OnUpdateTransform)
                .SetEase(Ease.Linear);
        }

        private void OnUpdateTransform(float percentage)
        {
            transform.SetPositionAndRotation(
                Vector3.LerpUnclamped(
                    _sourcePosition,
                    _destinationPosition,
                    _positionCurve.Evaluate(percentage)),
                Quaternion.LerpUnclamped(
                    _sourceRotation,
                    _destinationRotation,
                    _rotationCurve.Evaluate(percentage)));

            foreach (var limb in _limbs.Values)
            {
                limb.UpdateLimb();
            }
        }

        private Vector3 GetOffsetPosition()
        {
            var averagePosition = Vector3.zero;
            foreach (var limb in _limbs.Values)
            {
                averagePosition += limb.Grabber.transform.position;
            }

            return averagePosition / _limbs.Values.Count;
        }

        private void OnDestroy()
        {
            _hitbox.Initiated -= OnHitboxInitiated;

            foreach (var limb in _limbs.Values)
            {
                limb.Entered -= OnLimbEntered;
            }
        }
    }
}
