using System;
using Uphill.Scripts.Core;

namespace Uphill.Scripts.Consumables
{
    public abstract class Consumable : Visitable
    {
        public Action<Consumable> Consumed;

        protected bool _isConsuming { get; set; } = false;

        protected Grabber Grabber { get; set; } = default!;

        protected override bool IsValid(object visitor) => visitor is Grabber;

        protected override void Process(object visitor)
        {
            Grabber = (Grabber)visitor;
            Grabber.ConsumableVisit(this);

            _isConsuming = true;
        }

        protected void OnConsumed() => Consumed?.Invoke(this);
    }
}
