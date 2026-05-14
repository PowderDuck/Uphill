using System.Collections.Generic;
using UnityEngine;
using Uphill.Scripts.Climbables;

namespace Uphill.Scripts.Sections
{
    public abstract class SectionBase : MonoBehaviour, ISection
    {
        [field: SerializeField]
        public float OperatingHeight { get; private set; } = 0;

        [SerializeField] protected Vector2 _sectionSize = new(1, 2);
        [SerializeField] protected Vector2 _gridSize = new(3, 5);
        [SerializeField] protected List<Climbable> _climbablePrefabs = default!;

        protected readonly List<Climbable> _climbables = new();

        public virtual void Initialize()
        {
            var boxSize = new Vector2(
                _sectionSize.x / (_gridSize.x - 1f),
                _sectionSize.y / (_gridSize.y - 1f));

            for (var x = 0; x < _gridSize.x - 1; x++)
            {
                for (var y = 0; y < _gridSize.y - 1; y++)
                {
                    var offset = new Vector2(
                        Random.Range(-0.5f, 0.5f) * boxSize.x,
                        Random.Range(-0.5f, 0.5f) * boxSize.y);

                    var gridPosition = new Vector2(
                        (((x / (_gridSize.x - 1)) - 0.5f) * _sectionSize.x) + (boxSize.x / 2f),
                        (((y / (_gridSize.y - 1)) - 0.5f) * _sectionSize.y) + (boxSize.y / 2f));

                    var check = new GameObject();
                    check.transform.position = (Vector2)transform.position + gridPosition;
                    check.transform.SetParent(transform);

                    var climbableIndex = Random.Range(0, _climbablePrefabs.Count);
                    var climbableRotation = new Vector3(0, 0, Random.Range(0f, 360f));
                    var climbable = Instantiate(
                        _climbablePrefabs[climbableIndex],
                        (Vector2)transform.position + gridPosition + offset,
                        Quaternion.Euler(climbableRotation));

                    climbable.transform.SetParent(transform);

                    _climbables.Add(climbable);
                }
            }
        }

        public virtual void Update() { }

        public virtual void Enable() { }
        public virtual void Disable() { }
    }
}
