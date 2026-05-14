using Uphill.Scripts.Core;

namespace Uphill.Scripts.Climbables
{
    public abstract class Climbable : Visitable
    {
        protected override bool IsValid(object visitor) => visitor is Grabber;

        protected override void Process(object visitor) =>
            ((Grabber)visitor).ClimbableVisit(this);
    }
}
