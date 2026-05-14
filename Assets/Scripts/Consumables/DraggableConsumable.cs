using UnityEngine;

namespace Uphill.Scripts.Consumables
{
    public class DraggableConsumable : Consumable
    {
        private Vector3 _initialScale = Vector3.zero;

        protected virtual void Start()
        {
            _initialScale = transform.localScale;
        }

        protected override void Process(object visitor)
        {
            base.Process(visitor);

            Grabber.TransformUpdated += OnGrabberTransformUpdated;
        }

        protected virtual void OnGrabberTransformUpdated(float percentage)
        {
            transform.position = Grabber.transform.position;

            transform.localScale = Vector3.Lerp(
                _initialScale, Vector3.zero, percentage);
        }

        protected virtual void OnDestroy()
        {
            if (Grabber != null)
            {
                Grabber.TransformUpdated -= OnGrabberTransformUpdated;
            }
        }
    }
}
