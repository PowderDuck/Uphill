using UnityEngine;

namespace Uphill.Scripts.Sections
{
    public class SectionController : MonoBehaviour
    {
        [field: SerializeField]
        public SectionBase Section { get; private set; } = default!;
    }
}
