using System;
using UnityEngine;
using Uphill.Scripts.Climbables;

namespace Uphill.Scripts
{
    public class Grabber : MonoBehaviour
    {
        public Action<Climbable> Entered;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Climbable>(out var climbable))
            {
                Entered?.Invoke(climbable);
            }
        }
    }
}
