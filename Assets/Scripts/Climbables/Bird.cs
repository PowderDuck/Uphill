using DG.Tweening;
using UnityEngine;

namespace Uphill.Scripts.Climbables
{
    public class Bird : Climbable
    {
        [SerializeField] private float _flyingVelocity = 5f;

        [SerializeField] private AnimationCurve _swingCurve = default!;
        [SerializeField] private float _swingMagnitude = 0.5f;
        [SerializeField] private float _swingPeriod = 0.75f;

        private Vector3 _dynamicPosition { get; set; } = Vector3.zero;

        private void Start() => MoveBird();

        private void MoveBird()
        {
            _dynamicPosition = transform.position;

            DOVirtual
                .Float(0, 1, _swingPeriod, OnMove)
                .SetEase(Ease.Linear)
                .OnComplete(MoveBird);
        }

        private void OnMove(float percentage)
        {
            _dynamicPosition += _flyingVelocity * Time.deltaTime * transform.right;
            transform.position = _dynamicPosition
                + (_swingMagnitude * _swingCurve.Evaluate(percentage) * transform.up);
        }
    }
}
