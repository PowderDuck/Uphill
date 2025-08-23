using System;
using UnityEngine;
using Uphill.Scripts.Enums;

namespace Uphill.Scripts.Events
{
    public class LimbHitboxInitiatedEventArgs : EventArgs
    {
        public LimbType LimbType { get; }

        public Vector2 Delta { get; }

        public LimbHitboxInitiatedEventArgs(LimbType limbType, Vector2 delta)
        {
            LimbType = limbType;
            Delta = delta;
        }
    }
}
