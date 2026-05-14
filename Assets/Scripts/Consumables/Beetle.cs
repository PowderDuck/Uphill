using DG.Tweening;
using UnityEngine;
using Uphill.Scripts.Utils;

namespace Uphill.Scripts.Consumables
{
    public class Beetle : DraggableConsumable
    {
        [SerializeField] private float _crouchingSpeed = 1f;

        [SerializeField] private float _redirectPeriod = 2f;
        [SerializeField] private float _redirectRange = 30f;
        [SerializeField] private float _redirectSmoothness = 0.3f;

        private Vector2 _currentDirection = Vector2.zero;
        private float _currentDirectionAngle = 0f;
        private Vector3 _currentRotation = Vector3.zero;
        private Interpolator _interpolator = default!;

        private Rigidbody2D _rigidbody = default!;
        private Tweener _updateTweener = default!;

        protected override void Start()
        {
            base.Start();

            _rigidbody = GetComponent<Rigidbody2D>();

            _currentDirectionAngle = Random.Range(0, 360f);
            _interpolator = new Interpolator(_currentDirectionAngle, _currentDirectionAngle);

            MoveBeetle();
        }

        private void MoveBeetle()
        {
            ChangeDirection();

            _updateTweener = DOVirtual
                .Float(0, 1, _redirectPeriod, OnMove)
                .OnComplete(MoveBeetle);
        }

        private void OnMove(float _)
        {
            if (_isConsuming)
            {
                return;
            }

            _rigidbody.linearVelocity = _crouchingSpeed * _currentDirection;
        }

        private void ChangeDirection()
        {
            _currentDirectionAngle += Random.Range(-_redirectRange, _redirectRange);
            _interpolator.Update(_currentDirectionAngle);

            DOVirtual.Float(0, 1, _redirectSmoothness, OnDirectionChanged);
        }

        private void OnDirectionChanged(float percentage)
        {
            var angle = _interpolator.GetValue(percentage);

            _currentRotation.z = -angle;
            transform.eulerAngles = _currentRotation;

            _currentDirection.Set(
                Mathf.Sin(angle * Mathf.Deg2Rad),
                Mathf.Cos(angle * Mathf.Deg2Rad));
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
