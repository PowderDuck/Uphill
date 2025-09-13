using UnityEngine;

namespace Uphill.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Climber _climber = default!;

        [SerializeField] private float _adjustmentVelocity = 5;
        [SerializeField] private Vector2 _boundaries = new(0.8f, 0.8f);

        private Vector2 _cameraDimensions { get; set; } = Vector2.zero;

        private Vector2 _direction { get; set; } = Vector2.zero;
        private Vector3 _dynamicPosition = Vector3.zero;
        private bool _isAdjusting { get; set; } = false;

        private void Start()
        {
            var orthographicSize = Camera.main.orthographicSize;
            _cameraDimensions = new(
                (float)Camera.main.pixelWidth / Camera.main.pixelHeight * orthographicSize,
                orthographicSize);

            _dynamicPosition = transform.position;
        }

        private void FixedUpdate()
        {
            _direction = _climber.transform.position - transform.position;
            if (!_isAdjusting)
            {
                _isAdjusting = (Mathf.Abs(_direction.x) / _cameraDimensions.x) >= _boundaries.x
                    || (Mathf.Abs(_direction.y) / _cameraDimensions.y) >= _boundaries.y;
            }
            else
            {
                _dynamicPosition.Set(
                    _dynamicPosition.x + (_adjustmentVelocity * _direction.normalized.x),
                    _dynamicPosition.y + (_adjustmentVelocity * _direction.normalized.y),
                    _dynamicPosition.z);

                if (_direction.magnitude <= _adjustmentVelocity)
                {
                    _dynamicPosition.Set(
                        _climber.transform.position.x,
                        _climber.transform.position.y,
                        _dynamicPosition.z);

                    _isAdjusting = false;
                }

                transform.position = _dynamicPosition;
            }
        }
    }
}
