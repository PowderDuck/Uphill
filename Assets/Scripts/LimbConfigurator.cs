using UnityEngine;

namespace Uphill.Scripts
{
    public abstract class LimbConfigurator : ScriptableObject
    {
        [field: SerializeField]
        public Limb LeftArm { get; private set; } = default!;

        [field: SerializeField]
        public Limb RightArm { get; private set; } = default!;

        [field: SerializeField]
        public Limb LeftLeg { get; private set; } = default!;

        [field: SerializeField]
        public Limb RightLeg { get; private set; } = default!;
    }
}