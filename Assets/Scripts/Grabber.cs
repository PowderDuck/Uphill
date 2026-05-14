using System;
using UnityEngine;
using Uphill.Scripts.Climbables;
using Uphill.Scripts.Consumables;
using Uphill.Scripts.Core;

namespace Uphill.Scripts
{
    public class Grabber : MonoBehaviour
    {
        public event Action<Climbable>? ClimbableEntered;
        public event Action<Consumable>? ConsumableEntered;

        public event Action<float>? TransformUpdated;

        public void SetPosition(Vector3 targetPoint, float percentage)
        {
            transform.position = targetPoint;

            TransformUpdated?.Invoke(percentage);
        }

        public void ClimbableVisit(Visitable visitable) =>
            Visited(visitable, ClimbableEntered);
        public void ConsumableVisit(Visitable visitable) =>
            Visited(visitable, ConsumableEntered);

        private void Visited<TVisitable>(
            Visitable visitable,
            Action<TVisitable>? callback) where TVisitable : Visitable
        {
            callback?.Invoke((TVisitable)visitable);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Visitable>(out var visitable))
            {
                visitable.Visit(this);
            }
        }
    }
}
