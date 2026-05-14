using DG.Tweening;
using UnityEngine;

namespace Uphill.Scripts.Consumables
{
    public class Bird : DraggableConsumable
    {
        [SerializeField] private float _flyingVelocity = 5f;

        [SerializeField] private AnimationCurve _swingCurve = default!;
        [SerializeField] private float _swingMagnitude = 0.5f;
        [SerializeField] private float _swingPeriod = 0.75f;

        [SerializeField] private ParticleSystem _featherEffect = default!;

        private Vector3 _dynamicPosition { get; set; } = Vector3.zero;

        private Tweener _updateTweener { get; set; } = default!;

        protected override void Start()
        {
            base.Start();

            MoveBird();
        }

        private void MoveBird()
        {
            _dynamicPosition = transform.position;

            _updateTweener = DOVirtual
                .Float(0, 1, _swingPeriod, OnMove)
                .SetEase(Ease.Linear)
                .OnComplete(MoveBird);
        }

        private void OnMove(float percentage)
        {
            if (_isConsuming)
            {
                return;
            }

            _dynamicPosition += _flyingVelocity * Time.deltaTime * transform.right;
            transform.position = _dynamicPosition
                + (_swingMagnitude * _swingCurve.Evaluate(percentage) * transform.up);
        }

        protected override void Process(object visitor)
        {
            base.Process(visitor);

            Instantiate(
                _featherEffect,
                transform.position,
                _featherEffect.transform.rotation).Play();
        }

        protected override void OnGrabberTransformUpdated(float percentage)
        {
            base.OnGrabberTransformUpdated(percentage);

            if (percentage >= 1)
            {
                OnConsumed();
                _updateTweener?.Kill();

                Destroy(gameObject);
            }
        }
    }
}
