using UnityEngine;

namespace Uphill.Scripts.Core
{
    public abstract class Visitable : MonoBehaviour
    {
        public virtual void Visit(object visitor)
        {
            if (!IsValid(visitor))
            {
                return;
            }

            Process(visitor);
        }

        protected abstract bool IsValid(object visitor);

        protected virtual void Process(object visitor) { }
    }
}
