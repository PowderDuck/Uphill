using System.Collections.Generic;
using UnityEngine;
using Uphill.Scripts.Climbables;

namespace Uphill.Scripts.Sections
{
    public abstract class SectionBase : ScriptableObject, ISection
    {
        [field: SerializeField]
        public float Height { get; private set; }

        [field: SerializeField]
        public List<Climbable> Climbables { get; private set; }

        public virtual void Initialize() { }

        public virtual void Update() { }

        public virtual void Enable() { }
        public virtual void Disable() { }
    }
}
