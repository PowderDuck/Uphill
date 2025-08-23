using System;
using Uphill.Scripts.Enums;

namespace Uphill.Scripts.Events
{
    public class LimbEnteredEventArgs : EventArgs
    {
        public LimbType LimbType { get; }

        public LimbEnteredEventArgs(LimbType limbType)
        {
            LimbType = limbType;
        }
    }
}
