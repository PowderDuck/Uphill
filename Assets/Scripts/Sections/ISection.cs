using System.Collections.Generic;
using Uphill.Scripts.Climbables;

namespace Uphill.Scripts.Sections
{
    public interface ISection
    {
        float Height { get; }
        List<Climbable> Climbables { get; }

        void Initialize();

        void Update();
    }
}
