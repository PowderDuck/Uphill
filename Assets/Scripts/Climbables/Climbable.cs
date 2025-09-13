using UnityEngine;

namespace Uphill.Scripts.Climbables
{
    public abstract class Climbable : MonoBehaviour
    {
        public virtual void Visit(object visitor) { }
    }
}
