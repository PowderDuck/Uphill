using System;
using UnityEngine;

namespace Uphill.Scripts.Events
{
    public class LimbHitboxInitiatedEventArgs : EventArgs
    {
        public Vector2 Delta { get; }
        public Vector2 InitialPosition { get; }

        public double HoldDuration { get; }

        public LimbHitboxInitiatedEventArgs(
            Vector2 delta, Vector2 initialPosition, double holdDuration)
        {
            Delta = delta;
            InitialPosition = initialPosition;
            HoldDuration = holdDuration;
        }
    }
}
